using WGMansion.Api.Models.Stocks;
using WGMansion.MongoDB.Services;

namespace WGMansion.Api.ViewModels
{
    public interface IStocksViewModel
    {
        Task<Stock> GetStock(string symbol);
    }

    public class StocksViewModel : IStocksViewModel
    {
        private readonly IMongoService<Stock> _mongoService;
        private const string STOCKS_COLLECTION = "stocks";
        
        public StocksViewModel(IMongoService<Stock> mongoService)
        {
            _mongoService = mongoService;
        }

        public async Task<Stock> GetStock(string symbol)
        {
            _mongoService.SetCollection(STOCKS_COLLECTION);
            var result = await _mongoService.FindOneAsync(x=>x.Symbol == symbol);
            if (result == null) throw new Exception($"Could not find symbol: {symbol}");
            return result;
        }
    }
}
