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
        public async Task<ActionResult<Account>> Authenticate(string username, string password)
        {
            try
            {
                _logger.Info($"Authenticating User: {username}");
                var document = await _accountsViewModel.Authenticate(username, password);
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
        public async Task<ActionResult<Account>> CreateUser(string username, string password)
        {
            try
            {
                _logger.Info($"Creating User: {username}");
                var result = await _accountsViewModel.CreateAccount(username, password);
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
