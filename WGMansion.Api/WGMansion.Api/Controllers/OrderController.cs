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

        public OrderController(IOrderViewModel orderViewModel)
        {
            _orderViewModel = orderViewModel;
        }

        [HttpPost]
        [Route("/addorder")]
        public async Task<ActionResult<Order>> AddOrder([FromBody] Order order)
        {
            try
            {

            }
            catch(Exception e)
            {
                _logger.Error(e.ToString());
                return BadRequest(e.ToString());
            }
        }
    }
}
