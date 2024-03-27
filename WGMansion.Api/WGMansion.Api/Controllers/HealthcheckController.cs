using log4net;
using Microsoft.AspNetCore.Mvc;
using WGMansion.Api.ViewModels;

namespace WGMansion.Api.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class HealthcheckController : ControllerBase
    {
        private ILog _logger = LogManager.GetLogger(typeof(HealthcheckController));
        private readonly IHealthcheckViewModel _healthcheckViewModel;
        public HealthcheckController(IHealthcheckViewModel healthcheckViewModel)
        {
            _healthcheckViewModel = healthcheckViewModel;
        }

        [HttpGet]
        [Route("/healthcheck")]
        public ActionResult<string> Healthcheck()
        {
            try
            {
                _logger.Info("Pinging");
                var result = _healthcheckViewModel.Ping().ToString();
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
                return BadRequest(e.ToString());
            }
        }
    }
}
