using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _sut = new AccountsController(_accountsViewModel.Object);
        }

        [Test]
        public async Task TestAuthenticate()
        {
            var account = new Account();
            _accountsViewModel.Setup(x=>x.Authenticate("username", "password")).ReturnsAsync(account);
            var result = await _sut.Authenticate("username", "password");
            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task TestAuthenticateBadResult()
        {
            var account = new Account();
            _accountsViewModel.Setup(x => x.Authenticate("username", "password")).ThrowsAsync(new Exception());
            var result = await _sut.Authenticate("username", "password");
            var badResult = result.Result as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(badResult?.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task TestCreateUser()
        {
            var account = new Account();
            _accountsViewModel.Setup(x => x.CreateAccount("username", "password")).ReturnsAsync(account);
            var result = await _sut.CreateUser("username", "password");
            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task TestCreateUserBadResult()
        {
            var account = new Account();
            _accountsViewModel.Setup(x => x.CreateAccount("username", "password")).ThrowsAsync(new Exception());
            var result = await _sut.CreateUser("username", "password");
            var badResult = result.Result as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(badResult?.StatusCode, Is.EqualTo(400));
        }
    }
}
