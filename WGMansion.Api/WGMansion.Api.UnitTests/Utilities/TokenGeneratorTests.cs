using Microsoft.Extensions.Options;
using MongoDB.Driver.Linq;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using WGMansion.Api.Models;
using WGMansion.Api.Settings;
using WGMansion.Api.Utility;

namespace WGMansion.Api.UnitTests.Utilities
{
    [TestFixture]
    internal class TokenGeneratorTests
    {
        private TokenGenerator _sut;
        private Mock<IOptions<AppSettings>> _appSettings;

        [SetUp]
        public void Setup()
        {
            _appSettings = new Mock<IOptions<AppSettings>>();
            var appSettings = new AppSettings
            {
                Secret = "secret secret secret secret secret secret secret secret secret secret "
            };

            _appSettings.Setup(x => x.Value).Returns(appSettings);

            _sut = new TokenGenerator(_appSettings.Object);
        }

        [Test]
        public void TestGenerateToken()
        {
            var account = new Account
            {
                Id = "123",
                Role = "Admin"
            };

            var result = _sut.GetToken(account);
            var token = new JwtSecurityToken(result);
            var date = DateTime.UtcNow.AddDays(7);

            Assert.That(result, Is.Not.Null);
            Assert.That(token.Claims.First(x => x.Type == "role").Value, Is.EquivalentTo("Admin"));
            Assert.That(token.ValidTo.Date, Is.EqualTo(date.Date));
        }
    }
}
