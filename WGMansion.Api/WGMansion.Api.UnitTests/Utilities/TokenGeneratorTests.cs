using Microsoft.Extensions.Options;
using Moq;
using WGMansion.Api.Models;
using WGMansion.Api.Settings;
using WGMansion.Api.Utility;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.IdentityModel.Tokens.Jwt;
using MongoDB.Driver.Linq;

namespace WGMansion.Api.UnitTests.Utilities
{
    [TestFixture]
    internal class TokenGeneratorTests
    {
        private TokenGenerator _sut;
        private Mock<IOptions<AppSettings>> _appSettings = new Mock<IOptions<AppSettings>>();

        [SetUp]
        public void Setup()
        {
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
                Id = new ObjectId(),
                UserRole = "Admin"
            };

            var result = _sut.GetToken(account);
            var token = new JwtSecurityToken(result);
            var date = DateTime.UtcNow.AddDays(7);

            Assert.That(result,Is.Not.Null);
            Assert.That(token.Claims.First(x => x.Type == "role").Value, Is.EquivalentTo("Admin"));
            Assert.That(token.ValidTo.Date, Is.EqualTo(date.Date));
        }
    }
}
