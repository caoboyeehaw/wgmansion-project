using Microsoft.AspNetCore.Http;
using Moq;
using WGMansion.Api.ViewModels;
using WGMansion.MongoDB.Services;

namespace WGMansion.Api.UnitTests.ViewModels
{
    [TestFixture]
    internal class ImageViewModelTests
    {
        private ImageViewModel _sut;
        private Mock<IGridFSService> _gridFSService;

        [SetUp]
        public void Setup()
        {
            _gridFSService = new Mock<IGridFSService>();

            _sut = new ImageViewModel(_gridFSService.Object);
        }

        [Test]
        public async Task TestGetImage()
        {
            _gridFSService.Setup(x => x.DownloadAsBytesAsync(It.IsAny<string>())).ReturnsAsync(new byte[0]);

            var result = await _sut.GetImage("123");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task TestPostImage()
        {
            var image = new Mock<IFormFile>();
            image.Setup(x => x.FileName).Returns("123.png");
            _gridFSService.Setup(x => x.UploadFromBytesAsync(It.IsAny<string>(), It.IsAny<byte[]>())).ReturnsAsync("321");

            var result = await _sut.PostImage(image.Object, "123");

            _gridFSService.Verify(x => x.UploadFromBytesAsync(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo("321"));
        }

        [Test]
        public void TestPostImageInvalidFileType()
        {
            var image = new Mock<IFormFile>();
            image.Setup(x => x.FileName).Returns("123.exe");
            _gridFSService.Setup(x => x.UploadFromBytesAsync(It.IsAny<string>(), It.IsAny<byte[]>())).ReturnsAsync("321");

            var result = Assert.ThrowsAsync<Exception>(async () => await _sut.PostImage(image.Object, "123"));

            _gridFSService.Verify(x => x.UploadFromBytesAsync(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Never);
            Assert.That(result.Message, Is.EqualTo($"File extension not allowed 123.exe"));
        }

        [Test]
        public async Task TestDeleteImage()
        {
            await _sut.DeleteImage("123");
            _gridFSService.Verify(x => x.DeleteAsync("123"), Times.Once);
        }
    }
}
