using log4net;
using WGMansion.MongoDB.Services;

namespace WGMansion.Api.ViewModels
{
    public interface IImageViewModel
    {
        Task<byte[]> GetImage(string id);
    }

    public class ImageViewModel : IImageViewModel
    {
        private ILog _logger = LogManager.GetLogger(typeof(ImageViewModel));
        private readonly IGridFSService _gridFSService;

        public ImageViewModel(IGridFSService gridFSService) 
        { 
            _gridFSService = gridFSService;
        }

        public async Task<byte[]> GetImage(string id)
        {
            var result = await _gridFSService.DownloadAsBytesAsync(id);
            return result;
        }
    }
}
