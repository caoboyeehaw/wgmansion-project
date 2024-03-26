using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WGMansion.Api.Models;
using WGMansion.Api.Models.Stocks;
using WGMansion.Api.Models.Ticker;
using WGMansion.Api.ViewModels;

namespace WGMansion.Api.Controllers
{
    [Authorize(Roles = Roles.User)]
    [Route("v1/[controller]")]
    [ApiController]
    public class TickerController : ControllerBase
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(TickerController));
        private readonly ITickerViewModel _tickerViewModel;
        private readonly ITickerHistoryViewModel _tickerHistoryViewModel;

        public TickerController(ITickerViewModel tickerViewModel, ITickerHistoryViewModel tickerHistoryViewModel)
        {
            _tickerViewModel = tickerViewModel;
            _tickerHistoryViewModel = tickerHistoryViewModel;
        }

        [HttpGet]
        [Route("/ticker")]
        public async Task<ActionResult<Ticker>> GetTicker(string symbol)
        {
            try
            {
                _logger.Info($"Getting symbol {symbol}");
                var result = await _tickerViewModel.GetTicker(symbol);
                if (result == null) throw new Exception($"Could not find symbol: {symbol}");
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return BadRequest(e.ToString());
            }
        }

        [HttpGet]
        [Route("/alltickers")]
        public async Task<ActionResult<List<Ticker>>> GetAllTickers()
        {
            try
            {
                _logger.Info("Getting All Tickers");
                var result = await _tickerViewModel.GetAllTickers();
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return BadRequest(e.ToString());
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("/createticker")]
        public async Task<ActionResult<Ticker>> CreateTicker(string symbol)
        {
            try
            {
                _logger.Info($"Creating stock {symbol}");
                var result = await _tickerViewModel.CreateTicker(symbol);
                await _tickerHistoryViewModel.CreateTickerHistory(symbol);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return BadRequest(e.ToString());
            }
        }

        [HttpGet]
        [Route("/tickerhistory")]
        public async Task<ActionResult<TickerHistory>> GetTickerOrderHistory(string symbol, int page, int pageSize)
        {
            try
            {
                _logger.Info($"Get ticker history {symbol} | page {page} | page size {pageSize}");
                var result = await _tickerHistoryViewModel.GetTickerHistory(symbol, page, pageSize);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return BadRequest(e.ToString());
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("/addordertohistory")]
        public async Task<ActionResult<TickerHistory>> AddOrderToHistory([FromBody] Order order)
        {
            try
            {
                _logger.Info($"Adding order to ticker {order.Symbol}");
                var result = await _tickerHistoryViewModel.AddOrderToHistory(order);
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
