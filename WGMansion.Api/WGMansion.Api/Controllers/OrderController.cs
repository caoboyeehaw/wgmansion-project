using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WGMansion.Api.Models;
using WGMansion.Api.Models.Ticker;
using WGMansion.Api.ViewModels;

namespace WGMansion.Api.Controllers
{
    [Authorize(Roles = Roles.User)]
    [Route("v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(OrderController));
        private readonly IOrderViewModel _orderViewModel;

        public Func<string> GetUserId;

        public OrderController(IOrderViewModel orderViewModel)
        {
            _orderViewModel = orderViewModel;
            GetUserId = () => User.Identity.Name;
        }

        [HttpPost]
        [Route("/addorder")]
        public async Task<ActionResult<Order>> AddOrder(string symbol, float price, int quantity, OrderType orderType)
        {
            try
            {
                _logger.Info($"Adding order: {symbol} {orderType} ${price} #{quantity}");
                var result = await _orderViewModel.AddOrder(symbol, price, quantity, orderType, GetUserId());
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
                return BadRequest(e.ToString());
            }
        }

        [HttpDelete]
        [Route("/withdrawOrder")]
        public async Task<ActionResult<bool>> WithdrawOrder(string orderId, string tickerSymbol)
        {
            try
            {
                _logger.Info($"Withdrawing order {orderId}");
                await _orderViewModel.WithdrawOrder(orderId, tickerSymbol, GetUserId());
                return Ok(true);
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
                return BadRequest(e.ToString());
            }
        }
    }
}
