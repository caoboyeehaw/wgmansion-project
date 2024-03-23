using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGMansion.Api.Services;

namespace WGMansion.Api.UnitTests.Services
{
    [TestFixture]
    internal class EncryptionServiceTests
    {
        [Test]
        public void TestHashPassword()
        {
            var pass = EncryptionService.HashPassword("password");
            Assert.That(pass, Is.Not.EqualTo("password"));
            Assert.That(pass, Is.Not.Null);
        }

        [Test]
        public void TestVerifyPassword()
        {
            var pass = "password";
            var hashedPassword = EncryptionService.HashPassword(pass);
            var result = EncryptionService.VerifyPassword(pass, hashedPassword);
            Assert.That(result, Is.True);
        }

        [Test]
        public void TestWrongVerifyPassword()
        {
            var pass = "password";
            var hashedPassword = EncryptionService.HashPassword(pass);
            var result = EncryptionService.VerifyPassword("notpassword", hashedPassword);
            Assert.That(result, Is.False);
        }

    }
}
