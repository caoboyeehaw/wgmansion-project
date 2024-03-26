using Microsoft.AspNetCore.Mvc;
using Moq;
using WGMansion.Api.Controllers;
using WGMansion.Api.Models;
using WGMansion.Api.ViewModels;

namespace WGMansion.Api.UnitTests.Controllers
{
    [TestFixture]
    internal class HealthcheckControllerTests
    {
        private HealthcheckController _sut;
        private Mock<IHealthcheckViewModel> _healthcheckViewModel;

        [SetUp]
        public void Setup()
        {
            _healthcheckViewModel = new Mock<IHealthcheckViewModel>();
            _sut = new HealthcheckController(_healthcheckViewModel.Object);
        }

        [Test]
        public void TestPingHealthy()
        {
            _healthcheckViewModel.Setup(x => x.Ping()).Returns(HealthcheckResult.Healthy);
            var result = _sut.Healthcheck();
            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo("Healthy"));
        }

        [Test]
        public void TestPingDegraded()
        {
            _healthcheckViewModel.Setup(x => x.Ping()).Returns(HealthcheckResult.Degraded);
            var result = _sut.Healthcheck();
            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo("Degraded"));
        }

        [Test]
        public void TestPingDown()
        {
            _healthcheckViewModel.Setup(x => x.Ping()).Returns(HealthcheckResult.Down);
            var result = _sut.Healthcheck();
            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo("Down"));
        }

    }
}
