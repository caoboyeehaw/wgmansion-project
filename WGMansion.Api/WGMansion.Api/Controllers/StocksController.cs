using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WGMansion.Api.Models;
using WGMansion.Api.Models.Stocks;
using WGMansion.Api.ViewModels;

namespace WGMansion.Api.Controllers
{
    [Authorize(Roles = Roles.User)]
    [Route("v1/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(StocksController));
        private readonly IStocksViewModel _stocksViewModel;

        public StocksController(IStocksViewModel stocksViewModel) {
            _stocksViewModel = stocksViewModel;
        }

        [HttpGet]
        public async Task<ActionResult<Stock>> GetStock(string symbol)
        {
            try
            {
                _logger.Info($"Getting symbol {symbol}");
                var result = await _stocksViewModel.GetStock(symbol);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return BadRequest(e.ToString());
            }
        }
    }
}
