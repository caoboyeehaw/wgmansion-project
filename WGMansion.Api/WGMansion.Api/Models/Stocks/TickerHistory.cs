using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using WGMansion.MongoDB.Models;
using WGMansion.Api.Models.Ticker;

namespace WGMansion.Api.Models.Stocks
{
    public class TickerHistory : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("type")]
        public string Type { get; set; } = "ticker_history";
        [BsonElement("active")]
        public bool Active { get; set; }
        [BsonElement("symbol")]
        public string Symbol { get; set; }
        [BsonElement("orders")]
        public List<Order> Orders { get; set; }

    }
}
