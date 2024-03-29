namespace WGMansion.MongoDB.Services
{
    public interface IGridFSService
    {
        Task<string> UploadFromBytesAsync(string filename, byte[] bytes);
        Task<string> UploadFromStreamAsync(string filename, Stream stream);
        Task<byte[]> DownloadAsBytesAsync(string id);
        Task DeleteAsync(string id);
    }
}
