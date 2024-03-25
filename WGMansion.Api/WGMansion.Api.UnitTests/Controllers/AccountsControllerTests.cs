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
            _accountsViewModel.Setup(x=>x.Authenticate(account)).ReturnsAsync(account);
            var result = await _sut.Authenticate(account);
            var okResult = result.Result as OkObjectResult;

            Assert.That(result != null);
            Assert.That(okResult?.StatusCode == 200);
        }

        [Test]
        public async Task TestAuthenticateBadResult()
        {
            var account = new Account();
            _accountsViewModel.Setup(x => x.Authenticate(account)).ThrowsAsync(new Exception());
            var result = await _sut.Authenticate(account);
            var badResult = result.Result as BadRequestObjectResult;

            Assert.That(result != null);
            Assert.That(badResult?.StatusCode == 400);

        }

        [Test]
        public async Task TestCreateUser()
        {
            var account = new Account();
            _accountsViewModel.Setup(x => x.CreateAccount(account)).ReturnsAsync(account);
            var result = await _sut.CreateUser(account);
            var okResult = result.Result as OkObjectResult;

            Assert.That(result != null);
            Assert.That(okResult?.StatusCode == 200);
        }

        [Test]
        public async Task TestCreateUserBadResult()
        {
            var account = new Account();
            _accountsViewModel.Setup(x => x.CreateAccount(account)).ThrowsAsync(new Exception());
            var result = await _sut.CreateUser(account);
            var badResult = result.Result as BadRequestObjectResult;

            Assert.That(result != null);
            Assert.That(badResult?.StatusCode == 400);

        }
    }
}
