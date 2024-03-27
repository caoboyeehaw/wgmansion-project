using log4net;
using WGMansion.Api.Models.Stocks;
using WGMansion.Api.Models.Ticker;
using WGMansion.MongoDB.Services;

namespace WGMansion.Api.ViewModels
{
    public interface ITickerHistoryViewModel
    {
        Task<TickerHistory> CreateTickerHistory(string symbol);
        Task<TickerHistory> GetTickerHistory(string symbol, int page, int pageSize);
        Task<TickerHistory> AddOrderToHistory(Order order);
        Task<TickerHistory> AddOrderToHistory(List<Order> orders);
    }

    public class TickerHistoryViewModel : ITickerHistoryViewModel
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(TickerHistoryViewModel));
        private readonly IMongoService<TickerHistory> _mongoService;
        private const string TICKERS_COLLECTION = "tickers";
        private const string TICKER_HISTORY_TYPE = "ticker_history";
        private const int MAX_HISTORY = 100;

        public TickerHistoryViewModel(IMongoService<TickerHistory> mongoService)
        {
            _mongoService = mongoService;
        }

        public async Task<TickerHistory> CreateTickerHistory(string symbol)
        {
            _mongoService.SetCollection(TICKERS_COLLECTION);
            var tickerHistory = new TickerHistory
            {
                Type = TICKER_HISTORY_TYPE,
                Symbol = symbol,
                Orders = new List<Order>()
            };
            await _mongoService.InsertOneAsync(tickerHistory);
            return tickerHistory;
        }

        public async Task<TickerHistory> GetTickerHistory(string symbol, int page, int pageSize)
        {
            _mongoService.SetCollection(TICKERS_COLLECTION);
            var result = await _mongoService.FindOneAsync(x => x.Type == TICKER_HISTORY_TYPE && x.Symbol == symbol);
            result.Orders = result.Orders.Skip(page * pageSize).Take(pageSize).ToList();
            return result;
        }

        public async Task<TickerHistory> AddOrderToHistory(Order order)
        {
            _mongoService.SetCollection(TICKERS_COLLECTION);
            var result = await _mongoService.FindOneAsync(x => x.Type == TICKER_HISTORY_TYPE && x.Symbol == order.Symbol);
            result.Orders.Add(order);
            RemoveOverflowHistory(result);
            await _mongoService.ReplaceOneAsync(result);
            return result;
        }

        public async Task<TickerHistory> AddOrderToHistory(List<Order> orders)
        {
            if (orders.Count == 0) return null;
            _mongoService.SetCollection(TICKERS_COLLECTION);
            var result = await _mongoService.FindOneAsync(x => x.Type == TICKER_HISTORY_TYPE && x.Symbol == orders.First().Symbol);
            result.Orders.AddRange(orders);
            RemoveOverflowHistory(result);
            await _mongoService.ReplaceOneAsync(result);
            return result;
        }

        private static void RemoveOverflowHistory(TickerHistory tickerHistory)
        {
            if (tickerHistory.Orders.Count > MAX_HISTORY)
            {
                var overflow = tickerHistory.Orders.Count - MAX_HISTORY;
                tickerHistory.Orders = tickerHistory.Orders.Skip(overflow).Take(MAX_HISTORY).ToList();
            }
        }
    }
}
