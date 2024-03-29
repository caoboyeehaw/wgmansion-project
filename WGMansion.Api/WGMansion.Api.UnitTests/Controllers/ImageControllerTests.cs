using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WGMansion.Api.Controllers;
using WGMansion.Api.ViewModels;

namespace WGMansion.Api.UnitTests.Controllers
{
    [TestFixture]
    internal class ImageControllerTests
    {
        private ImageController _sut;
        private Mock<IImageViewModel> _imageViewModel;

        [SetUp]
        public void Setup()
        {
            _imageViewModel = new Mock<IImageViewModel>();

            _sut = new ImageController(_imageViewModel.Object)
            {
                GetUserId = () => "123"
            };
        }

        [Test]
        public async Task TestGetImage()
        {
            _imageViewModel.Setup(x => x.GetImage(It.IsAny<string>())).ReturnsAsync(new byte[0]);
            var result = await _sut.GetImage("123");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task TestGetImageException()
        {
            _imageViewModel.Setup(x => x.GetImage(It.IsAny<string>())).ThrowsAsync(new Exception());
            var result = await _sut.GetImage("123");
            var badResult = result.Result as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(badResult?.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task TestPostImage()
        {
            var image = new Mock<IFormFile>();
            _imageViewModel.Setup(x => x.PostImage(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync("123");
            var result = await _sut.PostImage(image.Object);
            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task TestPostImageException()
        {
            var image = new Mock<IFormFile>();
            _imageViewModel.Setup(x => x.PostImage(It.IsAny<IFormFile>(), It.IsAny<string>())).ThrowsAsync(new Exception());
            var result = await _sut.PostImage(image.Object);
            var badResult = result.Result as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(badResult?.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task TestDeleteImage()
        {
            _imageViewModel.Setup(x => x.DeleteImage("123"));
            var result = await _sut.DeleteImage("123");
            var okResult = result.Result as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }


        [Test]
        public async Task TestDeleteImageException()
        {
            _imageViewModel.Setup(x => x.DeleteImage("123")).ThrowsAsync(new Exception());
            var result = await _sut.DeleteImage("123");
            var badResult = result.Result as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(badResult?.StatusCode, Is.EqualTo(400));

        }
    }
}
