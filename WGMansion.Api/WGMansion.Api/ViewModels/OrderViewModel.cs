using log4net;
using MongoDB.Bson;
using System;
using WGMansion.Api.Models;
using WGMansion.Api.Models.Ticker;
using WGMansion.MongoDB.Services;

namespace WGMansion.Api.ViewModels
{
    public interface IOrderViewModel
    {
        Task<Order> AddOrder(Order order, string userId);
        Task WithdrawOrder(string orderId, string tickerSymbol, string userId);
    }

    public class OrderViewModel : IOrderViewModel
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(OrderViewModel));
        private readonly IAccountsViewModel _accountsViewModel;
        private readonly ITickerViewModel _tickerViewModel;

        public OrderViewModel(IAccountsViewModel accountsViewModel, ITickerViewModel tickerViewModel) 
        { 
            _accountsViewModel = accountsViewModel;
            _tickerViewModel = tickerViewModel;
        }

        public async Task<Order> AddOrder(Order order, string userId)
        {
            order.Id = ObjectId.GenerateNewId().ToString();
            order.OwnerId = userId;

            var account = await _accountsViewModel.GetAccount(userId);
            var ticker = await _tickerViewModel.GetTicker(order.Symbol);

            if (account == null) throw new Exception($"Account {userId} not found");
            if (ticker == null) throw new Exception($"Ticker {order.Symbol} not found");

            AddOrderToStock(order, account);
            ProcessOrderOnTicker(order, ticker);

            await _tickerViewModel.UpdateTicker(ticker);
            await _accountsViewModel.UpdateAccount(account);

            return order;
        }

        private void AddOrderToStock(Order order, Account account)
        {
            _logger.Info($"Adding order {order.Id} to {account.UserName}'s portfolio");
            var stock = account.Portfolio.Stocks.FirstOrDefault(x => x.Symbol == order.Symbol);

            if (stock == null) stock = CreateStock(account, order.Symbol);
            stock.Orders.Add(order.Id);
        }

        private Stock CreateStock(Account account, string symbol)
        {
            _logger.Info($"Adding {symbol} to {account.UserName}'s portfolio");
            var newStock = new Stock
            {
                Symbol = symbol
            };
            account.Portfolio.Stocks.Add(newStock);
            return newStock;
        }

        private async void ProcessOrderOnTicker(Order order, Ticker ticker)
        {
            switch (order.OrderType)
            {
                case OrderType.MarketBuy:
                    ticker.BuyOrders.Add(order);
                    var sellOrders = ticker.SellOrders.OrderBy(x => x.Price).ToList();
                    await BuyOrder(order, sellOrders, ticker);
                    break;
                case OrderType.LimitBuy:
                    ticker.BuyOrders.Add(order);
                    var limitSellOrders = ticker.SellOrders.Where(x => x.Price <= order.Price).OrderBy(x => x.Price).ToList();
                    await BuyOrder(order, limitSellOrders, ticker);
                    break;
                case OrderType.MarketSell:
                    ticker.SellOrders.Add(order);
                    var buyOrders = ticker.BuyOrders.OrderByDescending(x => x.Price).ToList();
                    await SellOrder(order, buyOrders, ticker);
                    break;
                case OrderType.LimitSell:
                    ticker.SellOrders.Add(order);
                    var limitBuyOrders = ticker.BuyOrders.Where(x => x.Price >= order.Price).OrderByDescending(x => x.Price).ToList();
                    await SellOrder(order, limitBuyOrders, ticker);
                    break;
                default:
                    throw new Exception($"Unexpected OrderType: {order.OrderType}");
            }
        }

        private async Task BuyOrder(Order buyOrder, List<Order> sellOrders, Ticker ticker)
        {
            var accountsToUpdate = new List<Account>();
            while(sellOrders.Count > 0 && buyOrder.Quantity > 0)
            {
                await FulfillOrder(buyOrder, sellOrders[0], ticker, accountsToUpdate);
                sellOrders.RemoveAll(x => x.Quantity == 0);
            }
            await UpdateAllAccounts(accountsToUpdate);
        }

        private async Task SellOrder(Order sellOrder, List<Order> buyOrders, Ticker ticker)
        {
            var accountsToUpdate = new List<Account>();
            while(buyOrders.Count > 0 && sellOrder.Quantity > 0)
            {
                sellOrder.Price = buyOrders[0].Price;
                await FulfillOrder(buyOrders[0],sellOrder, ticker, accountsToUpdate);
                buyOrders.RemoveAll(x => x.Quantity == 0);
            }
            await UpdateAllAccounts(accountsToUpdate);
        }

        private async Task UpdateAllAccounts(List<Account> accountsToUpdate)
        {
            foreach (var account in accountsToUpdate.Distinct())
            {
                await _accountsViewModel.UpdateAccount(account);
            }
        }

        private async Task FulfillOrder(Order buyOrder, Order sellOrder, Ticker ticker, List<Account> accountsToUpdate)
        {
            var buyer = await _accountsViewModel.GetAccount(buyOrder.OwnerId);
            var seller = await _accountsViewModel.GetAccount(sellOrder.OwnerId);
            var difference = Math.Min(buyOrder.Quantity, sellOrder.Quantity);

            buyer.Portfolio.Money -= difference * sellOrder.Price;
            seller.Portfolio.Money += difference * sellOrder.Price;

            buyOrder.Quantity -= difference;
            sellOrder.Quantity -= difference;

            RemoveOrderIfFulfilled(buyOrder, buyer, ticker);
            RemoveOrderIfFulfilled(sellOrder, seller, ticker);

            accountsToUpdate.Add(buyer);
            accountsToUpdate.Add(seller);

            _logger.Info($"{buyer.UserName} buys {difference} of {ticker.Symbol} from {seller.UserName} at {sellOrder.Price}");
        }

        private void RemoveOrderIfFulfilled(Order order, Account account, Ticker ticker) //TODO: record order history
        {
            if (order.Quantity != 0) return;
            account.Portfolio.Stocks.First(x => x.Symbol == order.Symbol).Orders.Remove(order.Id);
            if(order.OrderType == OrderType.MarketBuy || order.OrderType == OrderType.LimitBuy)
            {
                ticker.BuyOrders.Remove(order);
            }
            else if (order.OrderType == OrderType.MarketSell || order.OrderType == OrderType.LimitSell)
            {
                ticker.SellOrders.Remove(order);
            }
            _logger.Info($"{order.Id} fulfilled and removed");
        }

        public async Task WithdrawOrder(string orderId, string tickerSymbol, string userId)
        {
            var removeCount = 0;
            var ticker = await _tickerViewModel.GetTicker(tickerSymbol);
            removeCount += ticker.BuyOrders.RemoveAll(x=>x.Id == orderId);
            removeCount += ticker.SellOrders.RemoveAll(x => x.Id == orderId);
            var account = await _accountsViewModel.GetAccount(userId);
            account.Portfolio.Stocks.First(x => x.Symbol == tickerSymbol).Orders.Remove(orderId);

            await _accountsViewModel.UpdateAccount(account);
            await _tickerViewModel.UpdateTicker(ticker);

            if (removeCount == 0) _logger.Warn($"Warning: 0 counts of order {orderId} found in {tickerSymbol}");
            if (removeCount > 1) _logger.Warn($"Warning: more than one instance of order {orderId} removed from {tickerSymbol}");
        }
    }
}
