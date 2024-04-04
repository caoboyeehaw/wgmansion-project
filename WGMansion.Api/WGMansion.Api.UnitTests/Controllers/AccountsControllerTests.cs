using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WGMansion.Api.Controllers;
using WGMansion.Api.Models;
using WGMansion.Api.ViewModels;

namespace WGMansion.Api.UnitTests.Controllers
{
    [TestFixture]
    internal class AccountsControllerTests
    {
        private AccountsController _sut;
        private Mock<IAccountsViewModel> _accountsViewModel;

        [SetUp]
        public void Setup()
        {
            _accountsViewModel = new Mock<IAccountsViewModel>();
            _sut = new AccountsController(_accountsViewModel.Object)
            {
                GetUserId = () => "123"
            };
        }

        [Test]
        public async Task TestAuthenticate()
        {
            var account = new Account
            {
                UserName = "username",
                Password = "password",
            };
            _accountsViewModel.Setup(x => x.Authenticate("username", "password")).ReturnsAsync(account);
            var result = await _sut.Authenticate(account);
            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task TestAuthenticateBadResult()
        {
            var account = new Account
            {
                UserName = "username",
                Password = "password",
            };
            _accountsViewModel.Setup(x => x.Authenticate("username", "password")).ThrowsAsync(new Exception());
            var result = await _sut.Authenticate(account);
            var badResult = result.Result as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(badResult?.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task TestCreateUser()
        {
            var account = new Account
            {
                UserName = "username",
                Password = "password",
                Email = "email"
            };
            _accountsViewModel.Setup(x => x.CreateAccount("username", "password", "email")).ReturnsAsync(account);
            var result = await _sut.CreateUser(account);
            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task TestCreateUserBadResult()
        {
            var account = new Account
            {
                UserName = "username",
                Password = "password",
                Email = "email"
            };
            _accountsViewModel.Setup(x => x.CreateAccount("username", "password", "email")).ThrowsAsync(new Exception());
            var result = await _sut.CreateUser(account);
            var badResult = result.Result as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(badResult?.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task TestChangeProfilePicture()
        {
            var image = new Mock<IFormFile>();
            _accountsViewModel.Setup(x => x.ChangeProfilePicture(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync("123");
            var result = await _sut.ChangeProfilePicture(image.Object);
            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }
    }
}
