using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
