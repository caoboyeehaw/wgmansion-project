using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WGMansion.Api.Models;
using WGMansion.Api.ViewModels;

namespace WGMansion.Api.Controllers
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(AccountsController));
        private IAccountsViewModel _accountsViewModel;

        public AccountsController(IAccountsViewModel accountsViewModel)
        {
            _accountsViewModel = accountsViewModel;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<Account>> Authenticate([FromBody] Account user)
        {
            try
            {
                _logger.Info($"Authenticating User: {user.UserName}");
                var document = await _accountsViewModel.Authenticate(user);
                return Ok(document);
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
                return BadRequest(e.ToString());
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Account>> CreateUser([FromBody] Account user)
        {
            try
            {
                _logger.Info($"Creating User: {user.UserName}");
                var result = await _accountsViewModel.CreateAccount(user);
                _logger.Info($"Created {result.Id}");
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
