using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WGMansion.Api.Models;
using WGMansion.Api.Services;
using WGMansion.Api.Settings;
using WGMansion.Api.Utility;
using WGMansion.Api.ViewModels;
using WGMansion.MongoDB.Services;

namespace WGMansion.Api.UnitTests.ViewModels
{
    [TestFixture]
    internal class AccountsViewModelTests
    {
        private AccountsViewModel _sut;
        private Mock<IMongoService<Account>> _mongoService = new Mock<IMongoService<Account>>();
        private Mock<ITokenGenerator> _tokenGenerator = new Mock<ITokenGenerator>();

        [SetUp]
        public void Setup()
        {
            _tokenGenerator.Setup(x => x.GetToken(It.IsAny<Account>())).Returns("new token");
            _sut = new AccountsViewModel(_mongoService.Object, _tokenGenerator.Object);
        }

        [Test]
        public async Task TestAuthenticate()
        {
            var password = "password";
            var authUser = new Account
            {
                UserName = "username",
                Password = password
            };
            var dbUser = new Account
            {
                UserName = "username",
                Password = EncryptionService.HashPassword(password)
            };
            _mongoService.Setup( x =>  x.FindOneAsync(x => x.UserName == authUser.UserName)).ReturnsAsync(dbUser);

            var result = await _sut.Authenticate(authUser);

            _tokenGenerator.Verify(x => x.GetToken(dbUser), Times.Once);
            Assert.That(result.Password, Is.Null);
            Assert.That(result.Token, Is.EqualTo("new token"));
        }

        [Test]
        public void TestAuthenticateNoUser()
        {
            var password = "password";
            var authUser = new Account
            {
                UserName = "username",
                Password = password
            };
            _mongoService.Setup(x => x.FindOneAsync(x => x.UserName == authUser.UserName)).ReturnsAsync((Account)null);

            var result = Assert.ThrowsAsync<Exception>(async ()=>await _sut.Authenticate(authUser));
            Assert.That(result.Message, Is.EqualTo($"User not found {authUser.Id} {authUser.UserName}"));

        }

        [Test]
        public void TestAuthenticateWrongPassword()
        {
            var authUser = new Account
            {
                UserName = "username",
                Password = "password"
            };
            var dbUser = new Account
            {
                UserName = "username",
                Password = EncryptionService.HashPassword("wrong password")
            };
            _mongoService.Setup(x => x.FindOneAsync(x => x.UserName == authUser.UserName)).ReturnsAsync(dbUser);

            var result = Assert.ThrowsAsync<Exception>(async () => await _sut.Authenticate(authUser));
            Assert.That(result.Message, Is.EqualTo($"Wrong password for user {authUser.Id} {authUser.UserName}"));
        }

        [Test]
        public async Task TestCreateUser()
        {
            var newUser = new Account
            {
                UserName = "username",
                Password = "password"
            };

            _mongoService.Setup(x => x.FilterBy(x => x.UserName == newUser.UserName)).Returns(new List<Account>());

            var result = await _sut.CreateAccount(newUser);

            _mongoService.Verify(x => x.InsertOneAsync(It.IsAny<Account>()), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Password, Is.Null);
        }

        [Test]
        public void TestCreateUserAlreadyExists()
        {
            var newUser = new Account
            {
                UserName = "username",
                Password = "password"
            };

            _mongoService.Setup(x => x.FilterBy(It.IsAny<Expression<Func<Account,bool>>>())).Returns(new List<Account> { newUser });

            var result = Assert.ThrowsAsync<Exception>(async () => await _sut.CreateAccount(newUser));
            Assert.That(result.Message, Is.EqualTo($"User already exists : {newUser.UserName}"));
        }
    }
}
