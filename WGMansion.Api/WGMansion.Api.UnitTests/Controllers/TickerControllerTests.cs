using Microsoft.AspNetCore.Mvc;
using Moq;
using WGMansion.Api.Controllers;
using WGMansion.Api.Models.Stocks;
using WGMansion.Api.Models.Ticker;
using WGMansion.Api.ViewModels;

namespace WGMansion.Api.UnitTests.Controllers
{
    [TestFixture]
    internal class TickerControllerTests
    {
        private TickerController _sut;
        private Mock<ITickerViewModel> _tickerViewModel;
        private Mock<ITickerHistoryViewModel> _tickerHistoryViewModel;

        [SetUp]
        public void Setup()
        {
            _tickerViewModel = new Mock<ITickerViewModel>();
            _tickerHistoryViewModel = new Mock<ITickerHistoryViewModel>();
            _sut = new TickerController(_tickerViewModel.Object, _tickerHistoryViewModel.Object);
        }

        [Test]
        public async Task TestGetTicker()
        {
            var ticker = new Ticker
            {
                Symbol = "TEST"
            };
            _tickerViewModel.Setup(x => x.GetTicker(ticker.Symbol)).ReturnsAsync(ticker);

            var result = await _sut.GetTicker(ticker.Symbol);
            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task TestGetTickerNull()
        {
            var ticker = new Ticker
            {
                Symbol = "TEST"
            };
            Ticker nullTicker = null;
            _tickerViewModel.Setup(x => x.GetTicker(ticker.Symbol)).ReturnsAsync(nullTicker);

            var result = await _sut.GetTicker(ticker.Symbol);
            var badResult = result.Result as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(badResult?.StatusCode, Is.EqualTo(400));
        }


        [Test]
        public async Task TestGetTickerException()
        {
            var ticker = new Ticker
            {
                Symbol = "TEST"
            };
            _tickerViewModel.Setup(x => x.GetTicker(ticker.Symbol)).ThrowsAsync(new Exception());

            var result = await _sut.GetTicker(ticker.Symbol);
            var badResult = result.Result as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(badResult?.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task TestGetAllTickers()
        {
            _tickerViewModel.Setup(x => x.GetAllTickers()).ReturnsAsync(new List<Ticker>());

            var result = await _sut.GetAllTickers();
            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task TestGetAllTickersException()
        {
            _tickerViewModel.Setup(x => x.GetAllTickers()).ThrowsAsync(new Exception());

            var result = await _sut.GetAllTickers();
            var badResult = result.Result as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(badResult?.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task TestCreateTicker()
        {
            _tickerViewModel.Setup(x => x.CreateTicker("TEST")).ReturnsAsync(new Ticker());

            var result = await _sut.CreateTicker("TEST");
            _tickerHistoryViewModel.Verify(x => x.CreateTickerHistory("TEST"), Times.Once);
            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task TestCreateTickerException()
        {
            var ticker = new Ticker
            {
                Symbol = "TEST"
            };
            _tickerViewModel.Setup(x => x.CreateTicker("TEST")).ThrowsAsync(new Exception());

            var result = await _sut.GetTicker(ticker.Symbol);
            var badResult = result.Result as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(badResult?.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task TestGetTickerOrderHistory()
        {
            _tickerHistoryViewModel.Setup(x => x.GetTickerHistory("TEST", 0, 10)).ReturnsAsync(new TickerHistory());
            var result = await _sut.GetTickerOrderHistory("TEST", 0, 10);
            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task TTestGetTickerOrderHistoryException()
        {
            _tickerHistoryViewModel.Setup(x => x.GetTickerHistory("TEST", 0, 10)).ThrowsAsync(new Exception());
            var result = await _sut.GetTickerOrderHistory("TEST", 0, 10);
            var badResult = result.Result as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(badResult?.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task TestAddOrderToHistory()
        {
            var order = new Order();
            var history = new TickerHistory();
            _tickerHistoryViewModel.Setup(x => x.AddOrderToHistory(order)).ReturnsAsync(history);
            var result = await _sut.AddOrderToHistory(order);
            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task TestAddOrderToHistoryException()
        {
            var order = new Order();
            _tickerHistoryViewModel.Setup(x => x.AddOrderToHistory(order)).ThrowsAsync(new Exception());
            var result = await _sut.AddOrderToHistory(order);
            var badResult = result.Result as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(badResult?.StatusCode, Is.EqualTo(400));
        }
    }
}
