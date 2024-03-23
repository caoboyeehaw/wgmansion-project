using log4net;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using WGMansion.Api.Models;
using WGMansion.Api.Services;
using WGMansion.Api.Settings;
using WGMansion.Api.Utility;
using WGMansion.MongoDB.Services;

namespace WGMansion.Api.ViewModels
{
    public interface IAccountsViewModel
    {
        Task<Account> Authenticate(Account account);
        Task<Account> CreateAccount(Account account);
    }

    public class AccountsViewModel : IAccountsViewModel
    {
        private readonly IMongoService<Account> _mongoService;
        private readonly ITokenGenerator _tokenGenerator;
        private const string TypeValue = "User";
        private ILog _logger = LogManager.GetLogger(typeof(AccountsViewModel));

        public AccountsViewModel(IMongoService<Account> mongoService, ITokenGenerator tokenGenerator)
        {
            _mongoService = mongoService;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<Account> Authenticate(Account authUser)
        {
            _mongoService.SetCollection("accounts");
            var user = (await _mongoService.FindOneAsync(x => x.UserName == authUser.UserName));

            if (user == null)
                throw new Exception($"User not found {authUser.Id} {authUser.UserName}");
            if (!EncryptionService.VerifyPassword(authUser.Password, user.Password))
                throw new Exception($"Wrong password for user {authUser.Id} {authUser.UserName}");

            user.Token = _tokenGenerator.GetToken(user);
            user.Password = null;
            return user;
        }

        public async Task<Account> CreateAccount(Account account)
        {
            var newUser = new Account
            {
                UserName = account.UserName,
                Type = TypeValue,
                Password = EncryptionService.HashPassword(account.Password),
                UserRole = "User",
            };

            _mongoService.SetCollection("accounts");
            var findUser = _mongoService.FilterBy(x => x.UserName == newUser.UserName).ToList();
            if (findUser.Count > 0)
            {
                throw new Exception($"User already exists : {newUser.UserName}");
            }
            await _mongoService.InsertOneAsync(newUser);
            _logger.Info($"Created new user {newUser.UserName}");
            newUser.Password = null;
            return newUser;
        }
    }
}
