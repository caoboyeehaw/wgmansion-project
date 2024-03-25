using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WGMansion.Api.Models;
using WGMansion.Api.Models.Ticker;
using WGMansion.Api.ViewModels;
using WGMansion.MongoDB.Services;

namespace WGMansion.Api.UnitTests.ViewModels
{
    [TestFixture]
    internal class TickerViewModelTests
    {
        private TickerViewModel _sut;
        private Mock<IMongoService<Ticker>> _mongoService;

        [SetUp]
        public void Setup()
        {
            _mongoService = new Mock<IMongoService<Ticker>>();
            _sut = new TickerViewModel(_mongoService.Object);
        }

        [Test]
        public async Task TestGetTicker()
        {
            var ticker = new Ticker();
            _mongoService.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<Ticker, bool>>>())).ReturnsAsync(ticker);

            var result = await _sut.GetTicker("TEST");
            _mongoService.Verify(x=>x.SetCollection("tickers"),Times.Once());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void TestGetTickerNull()
        {
            Ticker? ticker = null;
            _mongoService.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<Ticker, bool>>>())).ReturnsAsync(ticker);
            Ticker? result = null;

            Assert.DoesNotThrowAsync(async ()=> result = await _sut.GetTicker("TEST"));
            _mongoService.Verify(x => x.SetCollection("tickers"), Times.Once());

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TestGetAllTickers()
        {
            var tickers = new List<Ticker>();
            _mongoService.Setup(x => x.FilterByAsync(It.IsAny<Expression<Func<Ticker, bool>>>())).ReturnsAsync(tickers);

            var result = await _sut.GetAllTickers();
            _mongoService.Verify(x => x.SetCollection("tickers"), Times.Once());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task TestCreateTicker()
        {
            Ticker ticker = null;
            _mongoService.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<Ticker, bool>>>())).ReturnsAsync(ticker);

            var result = await _sut.CreateTicker("TEST");
            _mongoService.Verify(x => x.SetCollection("tickers"), Times.Once());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Symbol, Is.EqualTo("TEST"));
        }

        [Test]
        public void TestCreateTickerSymbolTooLong()
        {
            var result = Assert.ThrowsAsync<Exception>(async () => await _sut.CreateTicker("TESTING"));
            Assert.That(result.Message, Is.EqualTo("Symbol length too long, max is 4 characters"));
        }

        [Test]
        public void TestCreateTickerSymbolTooShort()
        {
            var result = Assert.ThrowsAsync<Exception>(async () => await _sut.CreateTicker(""));
            Assert.That(result.Message, Is.EqualTo("Symbol length too short, min is 1 character"));
        }

        [Test]
        public async Task TestCreateTickerAlreadyExists()
        {
            var ticker = new Ticker();
            _mongoService.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<Ticker, bool>>>())).ReturnsAsync(ticker);

            var result = Assert.ThrowsAsync<Exception>(async () => await _sut.CreateTicker("TEST"));
            Assert.That(result.Message, Is.EqualTo($"Ticker already exists: TEST"));
        }
    }
}
