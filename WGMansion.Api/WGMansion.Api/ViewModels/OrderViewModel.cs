using log4net;
using MongoDB.Bson;
using WGMansion.Api.Models;
using WGMansion.Api.Models.Ticker;

namespace WGMansion.Api.ViewModels
{
    public interface IOrderViewModel
    {
        Task<Order> AddOrder(string symbol, float price, int quantity, OrderType orderType, string userId);
        Task WithdrawOrder(string orderId, string tickerSymbol, string userId);
    }

    public class OrderViewModel : IOrderViewModel
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(OrderViewModel));
        private readonly IAccountsViewModel _accountsViewModel;
        private readonly ITickerViewModel _tickerViewModel;
        private readonly ITickerHistoryViewModel _tickerHistoryViewModel;

        public OrderViewModel(IAccountsViewModel accountsViewModel, ITickerViewModel tickerViewModel, ITickerHistoryViewModel tickerHistoryViewModel)
        {
            _accountsViewModel = accountsViewModel;
            _tickerViewModel = tickerViewModel;
            _tickerHistoryViewModel = tickerHistoryViewModel;
        }

        public async Task<Order> AddOrder(string symbol, float price, int quantity, OrderType orderType, string userId)
        {
            var order = new Order
            {
                Id = ObjectId.GenerateNewId().ToString(),
                OrderType = orderType,
                Symbol = symbol,
                Price = price,
                Quantity = quantity,
                MaxQuantity = quantity,
                PostDate = DateTime.UtcNow,
                OwnerId = userId
            };

            var account = await _accountsViewModel.GetAccount(userId);
            var ticker = await _tickerViewModel.GetTicker(order.Symbol);

            if (account == null) throw new Exception($"Account {userId} not found");
            if (ticker == null) throw new Exception($"Ticker {order.Symbol} not found");

            AddOrderToStock(order, account);
            await _accountsViewModel.UpdateAccount(account);
            ProcessOrderOnTicker(order, ticker);
            await _tickerViewModel.UpdateTicker(ticker);

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
                    var sellOrders = ticker.SellOrders.Where(x => x.OrderType != OrderType.MarketSell).OrderBy(x => x.Price).ToList();
                    await BuyOrder(order, sellOrders, ticker);
                    break;
                case OrderType.LimitBuy:
                    ticker.BuyOrders.Add(order);
                    var limitSellOrders = ticker.SellOrders.Where(x => x.Price <= order.Price || order.OrderType == OrderType.MarketSell).OrderBy(x => x.Price).ToList();
                    await BuyOrder(order, limitSellOrders, ticker);
                    break;
                case OrderType.MarketSell:
                    ticker.SellOrders.Add(order);
                    var buyOrders = ticker.BuyOrders.Where(x => x.OrderType != OrderType.MarketBuy).OrderByDescending(x => x.Price).ToList();
                    await SellOrder(order, buyOrders, ticker);
                    break;
                case OrderType.LimitSell:
                    ticker.SellOrders.Add(order);
                    var limitBuyOrders = ticker.BuyOrders.Where(x => x.Price >= order.Price || order.OrderType == OrderType.MarketBuy).OrderByDescending(x => x.Price).ToList();
                    await SellOrder(order, limitBuyOrders, ticker);
                    break;
                default:
                    throw new Exception($"Unexpected OrderType: {order.OrderType}");
            }
        }

        private async Task BuyOrder(Order buyOrder, List<Order> sellOrders, Ticker ticker)
        {
            var accountsToUpdate = new List<Account>();
            var orderHistory = new List<Order>();
            while (sellOrders.Count > 0 && buyOrder.Quantity > 0)
            {
                await FulfillOrder(buyOrder, sellOrders[0], ticker, accountsToUpdate, orderHistory);
                sellOrders.RemoveAll(x => x.Quantity == 0);
            }
            await _accountsViewModel.UpdateAccount(accountsToUpdate);
            await _tickerHistoryViewModel.AddOrderToHistory(orderHistory);
        }

        private async Task SellOrder(Order sellOrder, List<Order> buyOrders, Ticker ticker)
        {
            var accountsToUpdate = new List<Account>();
            var orderHistory = new List<Order>();
            while (buyOrders.Count > 0 && sellOrder.Quantity > 0)
            {
                sellOrder.Price = buyOrders[0].Price;
                await FulfillOrder(buyOrders[0], sellOrder, ticker, accountsToUpdate, orderHistory);
                buyOrders.RemoveAll(x => x.Quantity == 0);
            }
            await _accountsViewModel.UpdateAccount(accountsToUpdate);
            await _tickerHistoryViewModel.AddOrderToHistory(orderHistory);
        }

        private async Task FulfillOrder(Order buyOrder, Order sellOrder, Ticker ticker, List<Account> accountsToUpdate, List<Order> orderHistory)
        {
            var buyer = await _accountsViewModel.GetAccount(buyOrder.OwnerId);
            var seller = await _accountsViewModel.GetAccount(sellOrder.OwnerId);
            var difference = Math.Min(buyOrder.Quantity, sellOrder.Quantity);

            UpdatePortfolioStock(buyer, sellOrder.Price, difference, buyOrder.Symbol);
            UpdatePortfolioStock(seller, sellOrder.Price, -difference, buyOrder.Symbol);

            buyOrder.Quantity -= difference;
            sellOrder.Quantity -= difference;

            RemoveOrderIfFulfilled(buyOrder, buyer, ticker, orderHistory);
            RemoveOrderIfFulfilled(sellOrder, seller, ticker, orderHistory);

            accountsToUpdate.Add(buyer);
            accountsToUpdate.Add(seller);

            _logger.Info($"{buyer.UserName} buys {difference} of {ticker.Symbol} from {seller.UserName} at {sellOrder.Price}");
        }

        private void UpdatePortfolioStock(Account account, float price, int quantity, string symbol)
        {
            var stock = account.Portfolio.Stocks.First(x => x.Symbol == symbol);
            stock.AveragePrice = (stock.AveragePrice * stock.Quantity + price * quantity) / (quantity + stock.Quantity);
            stock.Quantity += quantity;
            account.Portfolio.Money -= price * quantity;
        }

        private void RemoveOrderIfFulfilled(Order order, Account account, Ticker ticker, List<Order> orderHistory)
        {
            if (order.Quantity != 0) return;
            order.FulfillDate = DateTime.UtcNow;
            var portfolioStock = account.Portfolio.Stocks.First(x => x.Symbol == order.Symbol);
            portfolioStock.Orders.Remove(order.Id);
            portfolioStock.OrderHistory.Add(order);

            if (order.OrderType == OrderType.MarketBuy || order.OrderType == OrderType.LimitBuy)
            {
                ticker.BuyOrders.Remove(order);
            }
            else if (order.OrderType == OrderType.MarketSell || order.OrderType == OrderType.LimitSell)
            {
                ticker.SellOrders.Remove(order);
            }
            orderHistory.Add(order);
            _logger.Info($"{order.Id} fulfilled and removed");
        }

        public async Task WithdrawOrder(string orderId, string tickerSymbol, string userId)
        {
            var removeCount = 0;
            var ticker = await _tickerViewModel.GetTicker(tickerSymbol);
            removeCount += ticker.BuyOrders.RemoveAll(x => x.Id == orderId);
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
