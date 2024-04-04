using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WGMansion.MongoDB.Models;

namespace WGMansion.Api.Models.Ticker
{
    public class Ticker : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("type")]
        public string Type { get; set; } = "ticker";
        [BsonElement("active")]
        public bool Active { get; set; } = true;
        [BsonElement("symbol")]
        public string? Symbol { get; set; }
        [BsonElement("buyOrders")]
        public List<Order> BuyOrders { get; set; } = new List<Order>();
        [BsonElement("sellOrders")]
        public List<Order> SellOrders { get; set; } = new List<Order>();
    }
}
