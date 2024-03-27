using Moq;
using System.Linq.Expressions;
using WGMansion.Api.Models;
using WGMansion.Api.Services;
using WGMansion.Api.Utility;
using WGMansion.Api.ViewModels;
using WGMansion.MongoDB.Services;

namespace WGMansion.Api.UnitTests.ViewModels
{
    [TestFixture]
    internal class AccountsViewModelTests
    {
        private AccountsViewModel _sut;
        private Mock<IMongoService<Account>> _mongoService;
        private Mock<ITokenGenerator> _tokenGenerator;

        [SetUp]
        public void Setup()
        {
            _mongoService = new Mock<IMongoService<Account>>();
            _tokenGenerator = new Mock<ITokenGenerator>();

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
            _mongoService.Setup(x => x.FindOneAsync(x => x.UserName == "username")).ReturnsAsync(dbUser);

            var result = await _sut.Authenticate("username", "password");
            _mongoService.Verify(x=>x.ReplaceOneAsync(dbUser), Times.Once());
            _tokenGenerator.Verify(x => x.GetToken(dbUser), Times.Once);

            Assert.That(result.Password, Is.Null);
            Assert.That(result.Token, Is.EqualTo("new token"));
            Assert.That(result.LastLogin.ToShortDateString(), Is.EqualTo(DateTime.UtcNow.ToShortDateString()));
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
            _mongoService.Setup(x => x.FindOneAsync(x => x.UserName == "username")).ReturnsAsync((Account)null);

            var result = Assert.ThrowsAsync<Exception>(async () => await _sut.Authenticate("username", "password"));
            Assert.That(result.Message, Is.EqualTo($"User not found {authUser.UserName}"));

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
            _mongoService.Setup(x => x.FindOneAsync(x => x.UserName == "username")).ReturnsAsync(dbUser);

            var result = Assert.ThrowsAsync<Exception>(async () => await _sut.Authenticate("username", "password"));
            Assert.That(result.Message, Is.EqualTo($"Wrong password for user {authUser.Password}"));
        }

        [Test]
        public void TestAuthenticateBannedAccount()
        {
            var authUser = new Account
            {
                UserName = "username",
                Password = "password"
            };
            var dbUser = new Account
            {
                UserName = "username",
                Password = EncryptionService.HashPassword("password"),
                Active = false
            };
            _mongoService.Setup(x => x.FindOneAsync(x => x.UserName == "username")).ReturnsAsync(dbUser);

            var result = Assert.ThrowsAsync<Exception>(async () => await _sut.Authenticate("username", "password"));
            Assert.That(result.Message, Is.EqualTo($"User username inactive: Banned account?"));
        }


        [Test]
        public async Task TestCreateUser()
        {
            var newUser = new Account
            {
                UserName = "username",
                Password = "password",
                Email = "email"
            };

            _mongoService.Setup(x => x.FilterBy(x => x.UserName == "username")).Returns(new List<Account>());

            var result = await _sut.CreateAccount("username", "password", "email");

            _mongoService.Verify(x => x.InsertOneAsync(newUser), Times.Once);

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

            _mongoService.Setup(x => x.FilterByAsync(It.IsAny<Expression<Func<Account, bool>>>())).ReturnsAsync(new List<Account> { newUser });

            var result = Assert.ThrowsAsync<Exception>(async () => await _sut.CreateAccount("username", "password", "email"));
            Assert.That(result.Message, Is.EqualTo($"User already exists : {newUser.UserName}"));
        }
    }
}
