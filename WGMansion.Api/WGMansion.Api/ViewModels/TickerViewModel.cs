using log4net;
using WGMansion.Api.Models.Ticker;
using WGMansion.MongoDB.Services;

namespace WGMansion.Api.ViewModels
{
    public interface ITickerViewModel
    {
        Task<Ticker> GetTicker(string symbol);
        Task<List<Ticker>> GetAllTickers();
        Task<Ticker> CreateTicker(string symbol);
        Task<Ticker> UpdateTicker(Ticker ticker);
    }

    public class TickerViewModel : ITickerViewModel
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(TickerViewModel));
        private readonly IMongoService<Ticker> _mongoService;
        private const string TICKERS_COLLECTION = "tickers";
        private const string TICKER_TYPE = "ticker";

        public TickerViewModel(IMongoService<Ticker> mongoService)
        {
            _mongoService = mongoService;
        }

        public async Task<Ticker> GetTicker(string symbol)
        {
            _mongoService.SetCollection(TICKERS_COLLECTION);
            var result = await _mongoService.FindOneAsync(x => x.Type == TICKER_TYPE && x.Symbol == symbol);
            return result;
        }

        public async Task<List<Ticker>> GetAllTickers()
        {
            _mongoService.SetCollection(TICKERS_COLLECTION);
            var result = await _mongoService.FilterByAsync(x => x.Type == TICKER_TYPE);
            return result.ToList();
        }

        public async Task<Ticker> CreateTicker(string symbol)
        {
            if (symbol.Length > 4) throw new Exception("Symbol length too long, max is 4 characters");
            if (symbol.Length == 0) throw new Exception("Symbol length too short, min is 1 character");
            var existingDoc = await GetTicker(symbol);
            if (existingDoc != null) throw new Exception($"Ticker already exists: {symbol}");

            var ticker = new Ticker { Symbol = symbol };

            await _mongoService.InsertOneAsync(ticker);
            return ticker;
        }

        public async Task<Ticker> UpdateTicker(Ticker ticker)
        {
            _mongoService.SetCollection(TICKERS_COLLECTION);
            await _mongoService.ReplaceOneAsync(ticker);
            return ticker;
        }
    }
}
