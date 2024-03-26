using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WGMansion.Api.Models.Ticker;
using WGMansion.MongoDB.Models;

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
        public List<Order> Orders { get; set; } = new List<Order>();

    }
}
