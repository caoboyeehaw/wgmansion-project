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
        Task<Account> Authenticate(string username, string password);
        Task<Account> CreateAccount(string username, string password);
    }

    public class AccountsViewModel : IAccountsViewModel
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(AccountsViewModel));
        private readonly IMongoService<Account> _mongoService;
        private readonly ITokenGenerator _tokenGenerator;
        private const string TYPE_VALUE = "User";
        private const string ACCOUNTS_COLLECTION = "accounts";

        public AccountsViewModel(IMongoService<Account> mongoService, ITokenGenerator tokenGenerator)
        {
            _mongoService = mongoService;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<Account> Authenticate(string username, string password)
        {
            _mongoService.SetCollection(ACCOUNTS_COLLECTION);
            var user = (await _mongoService.FindOneAsync(x => x.UserName == username));

            if (user == null)
                throw new Exception($"User not found {username}");
            if (!EncryptionService.VerifyPassword(password, user.Password))
                throw new Exception($"Wrong password for user {password}");

            user.Token = _tokenGenerator.GetToken(user);
            user.Password = null;
            return user;
        }

        public async Task<Account> CreateAccount(string username, string password)
        {
            var newUser = new Account
            {
                UserName = username,
                Type = TYPE_VALUE,
                Password = EncryptionService.HashPassword(password),
                Role = "User",
            };

            _mongoService.SetCollection(ACCOUNTS_COLLECTION);
            var findUser = _mongoService.FilterBy(x => x.UserName == username).ToList();
            if (findUser.Count > 0)
            {
                throw new Exception($"User already exists : {username}");
            }
            await _mongoService.InsertOneAsync(newUser);
            _logger.Info($"Created new user {username}");
            newUser.Password = null;
            return newUser;
        }
    }
}
