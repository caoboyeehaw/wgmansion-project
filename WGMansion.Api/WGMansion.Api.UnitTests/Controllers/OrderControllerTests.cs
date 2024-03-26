using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WGMansion.Api.Controllers;
using WGMansion.Api.Models.Ticker;
using WGMansion.Api.ViewModels;

namespace WGMansion.Api.UnitTests.Controllers
{
    [TestFixture]
    internal class OrderControllerTests
    {
        private OrderController _sut;
        private Mock<IOrderViewModel> _orderViewModel;

        [SetUp]
        public void Setup() 
        {
            _orderViewModel = new Mock<IOrderViewModel>();
            _sut = new OrderController(_orderViewModel.Object)
            {
               GetUserId = () => "123"
            };
        }

        [Test]
        public async Task TestAddOrder()
        {
            var order = new Order();
            _orderViewModel.Setup(x => x.AddOrder(order, "123")).ReturnsAsync(order);
            var result = await _sut.AddOrder(order);
            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task TestAddOrderException()
        {
            var order = new Order();
            _orderViewModel.Setup(x => x.AddOrder(order, "123")).ThrowsAsync(new Exception());
            var result = await _sut.AddOrder(order);
            var badResult = result.Result as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(badResult?.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task TestWithdrawOrder()
        {
            _orderViewModel.Setup(x => x.WithdrawOrder("123", "ABC", "123"));
            var result = await _sut.WithdrawOrder("123", "ABC");

            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }
    }
}
