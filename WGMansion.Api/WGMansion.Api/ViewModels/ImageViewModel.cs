using log4net;
using WGMansion.Api.Utility;
using WGMansion.MongoDB.Services;

namespace WGMansion.Api.ViewModels
{
    public interface IImageViewModel
    {
        Task<byte[]> GetImage(string id);
        Task<string> PostImage(IFormFile image, string userId);
        Task DeleteImage(string id);
    }

    public class ImageViewModel : IImageViewModel
    {
        private ILog _logger = LogManager.GetLogger(typeof(ImageViewModel));
        private readonly IGridFSService _gridFSService;
        private readonly string[] ALLOWED_EXTENSIONS = [".png", ".jpeg"];

        public ImageViewModel(IGridFSService gridFSService) 
        { 
            _gridFSService = gridFSService;
        }

        private bool IsFileExtensionAllowed(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            return ALLOWED_EXTENSIONS.Contains(extension);
        }

        public async Task<byte[]> GetImage(string id)
        {
            var result = await _gridFSService.DownloadAsBytesAsync(id);
            return result;
        }

        public async Task<string> PostImage(IFormFile image, string userId)
        {
            if (!IsFileExtensionAllowed(image)) throw new Exception($"File extension not allowed {image.FileName}");
            var bytes = await image.GetBytesAsync();
            var result = await _gridFSService.UploadFromBytesAsync(userId, bytes);
            return result;
        }

        public async Task DeleteImage(string id)
        {
            await _gridFSService.DeleteAsync(id);
        }
    }
}
