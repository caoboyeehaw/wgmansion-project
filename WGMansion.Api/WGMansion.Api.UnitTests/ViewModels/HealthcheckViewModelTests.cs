using Moq;
using WGMansion.Api.Models;
using WGMansion.Api.ViewModels;
using WGMansion.MongoDB.Services;

namespace WGMansion.Api.UnitTests.ViewModels
{
    [TestFixture]
    internal class HealthcheckViewModelTests
    {
        private HealthcheckViewModel _sut;
        private Mock<IMongoService<Account>> _mongoService;

        [SetUp]
        public void Setup()
        {
            _mongoService = new Mock<IMongoService<Account>>();
            _sut = new HealthcheckViewModel(_mongoService.Object);
        }

        [Test]
        public void TestGetHealthy()
        {
            _mongoService.Setup(x => x.Ping()).Returns(3);
            var result = _sut.Ping();
            Assert.That(result, Is.EqualTo(HealthcheckResult.Healthy));
        }

        [Test]
        public void TestGetDegraded()
        {
            _mongoService.Setup(x => x.Ping()).Returns(1);
            var result = _sut.Ping();
            Assert.That(result, Is.EqualTo(HealthcheckResult.Degraded));
        }

        [Test]
        public void TestGetDown()
        {
            _mongoService.Setup(x => x.Ping()).Returns(0);
            var result = _sut.Ping();
            Assert.That(result, Is.EqualTo(HealthcheckResult.Down));
        }
    }
}
