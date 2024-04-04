using log4net;
using Microsoft.Extensions.Options;
using WGMansion.Api.Models;
using WGMansion.Api.Models.Ticker;
using WGMansion.Bot.Services;
using WGMansion.Bot.Settings;

namespace WGMansion.Bot.Business
{
    public interface ITradeBot
    {

    }

    public class TradeBot : ITradeBot
    {
        private ILog _logger = LogManager.GetLogger(typeof(TradeBot));
        private readonly IApiService _apiService;
        private readonly BotSettings _botSettings;
        private Account _account;

        public TradeBot(IApiService apiService, IOptions<BotSettings> botSettings)
        {
            _apiService = apiService;
            _botSettings = botSettings.Value;
        }

        public void Start()
        {
            Task.Run(Update);
        }

        private async Task Update()
        {
            try
            {
                _account = await Login();
                while (true)
                {
                    if (_account.Portfolio.Stocks.Select(x => x.Orders).Count() > _botSettings.MaxOrders) continue;
                    var ticker = await PickTicker();
                    await PlaceOrder(ticker);

                    Thread.Sleep(_botSettings.CycleRateSeconds * 1000);
                    //_account = await _apiService.Get<Account>("getaccount", "", _account.Token);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
            }
        }

        private async Task<Account> Login()
        {
            _logger.Info("Logging in");
            var account = new Account
            {
                Id = "",
                UserName = _botSettings.Username,
                Password = _botSettings.Password,
                Email = "",
                ProfilePictureId = "",
                Role = "",
                Token = ""
            };

            return await _apiService.Post("authenticate", account, "");
        }

        private async Task<Ticker> PickTicker()
        {
            _logger.Info("Picking ticker");
            var tickers = await _apiService.Get<List<Ticker>>("alltickers", "", _account.Token);
            var random = new Random();
            var ticker = tickers[random.Next(0, tickers.Count)];
            _logger.Info($"Selected {ticker.Symbol}");
            return ticker;
        }

        private async Task PlaceOrder(Ticker ticker)
        {
            var newOrder = new Order
            {
                OrderType = PickOrderType(),
                Quantity = PickQuantity(),
                OwnerId = "",
                FulfillDate = DateTime.MinValue,
                Symbol = ticker.Symbol,
            };
            newOrder.Price = PickPrice(newOrder.OrderType, ticker);
            _logger.Info($"Placing order: {newOrder.Symbol} {newOrder.OrderType} ${newOrder.Price} #{newOrder.Quantity}");

            await _apiService.Post("addorder", newOrder, _account.Token);
        }

        private OrderType PickOrderType()
        {
            var random = new Random();
            if (random.Next(0, 100) < _botSettings.MarketOrderChance)
            {
                if (random.Next(0, 100) > 50)
                {
                    return OrderType.MarketBuy;
                }
                else
                {
                    return OrderType.MarketSell;
                }
            }
            else
            {
                if (random.Next(0, 100) > 50)
                {
                    return OrderType.LimitBuy;
                }
                else
                {
                    return OrderType.LimitSell;
                }
            }
        }

        private float PickPrice(OrderType orderType, Ticker ticker)
        {
            var random = new Random();
            if (orderType == OrderType.LimitBuy)
            {
                var lowestPrice = ticker.BuyOrders.Where(x=>x.OrderType == OrderType.LimitSell).OrderByDescending(x => x.Price).FirstOrDefault();
                if (lowestPrice == null) return 100;
                return lowestPrice.Price * (1 - (random.NextSingle() * _botSettings.LimitMargin));
            }
            else if (orderType == OrderType.LimitSell)
            {
                var highestPrice = ticker.SellOrders.Where(x=>x.OrderType == OrderType.LimitBuy).OrderBy(x => x.Price).FirstOrDefault();
                if (highestPrice == null) return 120;
                return highestPrice.Price * (random.NextSingle() * _botSettings.LimitMargin + 1);  

            }
            return 0;
        }

        private int PickQuantity()
        {
            var random = new Random();
            return random.Next(50, 100);
        }
    }
}
