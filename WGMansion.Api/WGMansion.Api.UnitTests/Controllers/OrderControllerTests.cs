using Microsoft.AspNetCore.Mvc;
using Moq;
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
            var order = new Order()
            {
                Symbol = "ABC",
                Price = 100,
                Quantity = 100,
                OrderType = OrderType.MarketBuy
            };
            _orderViewModel.Setup(x => x.AddOrder(It.IsAny<string>(), It.IsAny<float>(), It.IsAny<int>(), It.IsAny<OrderType>(), "123")).ReturnsAsync(order);
            var result = await _sut.AddOrder(order);
            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task TestAddOrderException()
        {
            var order = new Order()
            {
                Symbol = "ABC",
                Price = 100,
                Quantity = 100,
                OrderType = OrderType.MarketBuy
            };
            _orderViewModel.Setup(x => x.AddOrder(It.IsAny<string>(), It.IsAny<float>(), It.IsAny<int>(), It.IsAny<OrderType>(), It.IsAny<string>())).ThrowsAsync(new Exception());
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
