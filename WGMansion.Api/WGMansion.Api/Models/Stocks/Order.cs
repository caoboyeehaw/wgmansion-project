using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using WGMansion.MongoDB.Models;

namespace WGMansion.Api.Models.Ticker
{
    public class Order
    {
        [BsonElement("ownerId")]
        public ObjectId OwnerId { get; set; }
        [BsonElement("orderType")]
        public OrderType OrderType { get; set; }
        [BsonElement("symbol")]
        public string Symbol { get; set; }
        [BsonElement("price")]
        public float Price { get; set; }
        [BsonElement("quantity")]
        public int Quantity { get; set; }
    }
}
