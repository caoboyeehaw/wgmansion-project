﻿using log4net;
using WGMansion.Api.Models;
using WGMansion.Api.Services;
using WGMansion.Api.Utility;
using WGMansion.MongoDB.Services;

namespace WGMansion.Api.ViewModels
{
    public interface IAccountsViewModel
    {
        Task<Account> Authenticate(string username, string password);
        Task<Account> CreateAccount(string username, string password);
        Task<Account> GetAccount(string id);
        Task<Account> UpdateAccount(Account account);
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
            var findUser = (await _mongoService.FilterByAsync(x => x.UserName == username)).ToList();
            if (findUser.Count > 0)
            {
                throw new Exception($"User already exists : {username}");
            }
            await _mongoService.InsertOneAsync(newUser);
            _logger.Info($"Created new user {username}");
            newUser.Password = null;
            return newUser;
        }

        public async Task<Account> GetAccount(string id)
        {
            _mongoService.SetCollection(ACCOUNTS_COLLECTION);
            var user = await _mongoService.FindByIdAsync(id);
            return user;
        }

        public async Task<Account> UpdateAccount(Account account)
        {
            _mongoService.SetCollection(ACCOUNTS_COLLECTION);
            await _mongoService.ReplaceOneAsync(account);
            return account;
        }
    }
}
