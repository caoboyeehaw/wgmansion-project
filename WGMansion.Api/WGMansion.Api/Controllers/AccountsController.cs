using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WGMansion.Api.Models;
using WGMansion.Api.Utility;
using WGMansion.Api.ViewModels;

namespace WGMansion.Api.Controllers
{
    [Authorize(Roles = Roles.User)]
    [Route("v1/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(AccountsController));
        private IAccountsViewModel _accountsViewModel;
        public Func<string> GetUserId;

        public AccountsController(IAccountsViewModel accountsViewModel)
        {
            _accountsViewModel = accountsViewModel;
            GetUserId = () => User.Identity.Name;
        }

        [HttpGet]
        [Route("/getaccount")]
        public async Task<ActionResult<Account>> GetAccount()
        {
            try
            {
                _logger.Info($"Getting account {GetUserId()}");
                var result = await _accountsViewModel.GetAccount(GetUserId());
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return BadRequest(e);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/authenticate")]
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
        [Route("/createuser")]
        public async Task<ActionResult<Account>> CreateUser(string username, string password, string email)
        {
            try
            {
                _logger.Info($"Creating User: {username}");
                var result = await _accountsViewModel.CreateAccount(username, password, email);
                _logger.Info($"Created {result.Id}");
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
                return BadRequest(e.ToString());
            }
        }

        [HttpPost]
        [Route("/changepicture")]
        [RequestSizeLimit(1_000_000)]
        public async Task<ActionResult<string>> ChangeProfilePicture(IFormFile image)
        {
            try
            {
                var result = await _accountsViewModel.ChangeProfilePicture(image, GetUserId());
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
