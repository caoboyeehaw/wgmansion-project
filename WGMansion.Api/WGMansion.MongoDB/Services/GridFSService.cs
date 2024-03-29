using log4net;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using WGMansion.MongoDB.Settings;

namespace WGMansion.MongoDB.Services
{
    public class GridFSService : IGridFSService
    {
        private MongoClient _client;
        private IMongoDatabase _database;
        private MongoSettings _mongoSettings;
        private IGridFSBucket _bucket;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(GridFSService));

        public GridFSService(IOptions<MongoSettings> mongoSettings)
        {
            _logger.Info($"Connecting to Mongo GridFS...");
            _mongoSettings = mongoSettings.Value;
            _client = new MongoClient($"mongodb+srv://{_mongoSettings.Account}:{_mongoSettings.Password}@{_mongoSettings.Url}?retryWrites=true&w=majority");
            _database = _client.GetDatabase($"{_mongoSettings.Database}");
            _bucket = new GridFSBucket(_database);
            _logger.Info($"Connected.");
        }

        public async Task<string> UploadFromBytesAsync(string filename, byte[] bytes)
        {
            var result = await _bucket.UploadFromBytesAsync(filename, bytes);
            return result.ToString();
        }

        public async Task<string> UploadFromStreamAsync(string filename, Stream stream)
        {
            var result = await _bucket.UploadFromStreamAsync(filename, stream);
            return result.ToString();
        }

        public async Task<byte[]> DownloadAsBytesAsync(string id)
        {
            var result = await _bucket.DownloadAsBytesAsync(ObjectId.Parse(id));
            return result;
        }

        public async Task DeleteAsync(string id)
        {
            await _bucket.DeleteAsync(ObjectId.Parse(id));
        }
    }
}
