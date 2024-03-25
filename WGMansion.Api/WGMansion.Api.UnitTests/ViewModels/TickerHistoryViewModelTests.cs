using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WGMansion.Api.Models.Stocks;
using WGMansion.Api.Models.Ticker;
using WGMansion.Api.ViewModels;
using WGMansion.MongoDB.Services;

namespace WGMansion.Api.UnitTests.ViewModels
{
    [TestFixture]
    internal class TickerHistoryViewModelTests
    {
        private TickerHistoryViewModel _sut;
        private Mock<IMongoService<TickerHistory>> _mongoService;

        [SetUp]
        public void Setup()
        {
            _mongoService = new Mock<IMongoService<TickerHistory>>();
            _sut = new TickerHistoryViewModel(_mongoService.Object);
        }

        [Test]
        public async Task TestCreateTickerHistory()
        {
            _mongoService.Setup(x => x.InsertOneAsync(It.IsAny<TickerHistory>()));

            var result = await _sut.CreateTickerHistory("TEST");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Type, Is.EqualTo("ticker_history"));
            Assert.That(result.Symbol, Is.EqualTo("TEST"));
        }

        [Test]
        public async Task TestGetTickerHistory()
        {
            var history = new TickerHistory
            {
                Orders = new List<Order>
                {
                    new Order(),
                    new Order(),
                    new Order(),
                    new Order(),
                }
            };
            _mongoService.Setup(x=> x.FindOneAsync(It.IsAny<Expression<Func<TickerHistory, bool>>>())).ReturnsAsync(history);

            var result = await _sut.GetTickerHistory("TEST", 0, 4);

            Assert.That(result,Is.Not.Null);
            Assert.That(result.Orders.Count, Is.EqualTo(4));
        }

        [Test]
        public async Task TestGetTickerHistoryPagination()
        {
            var history = new TickerHistory
            {
                Orders = new List<Order>
                {
                    new Order(),
                    new Order(),
                    new Order(),
                    new Order(),
                }
            };
            _mongoService.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<TickerHistory, bool>>>())).ReturnsAsync(history);

            var result = await _sut.GetTickerHistory("TEST", 1, 3);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Orders.Count, Is.EqualTo(1));
        }
    }
}
