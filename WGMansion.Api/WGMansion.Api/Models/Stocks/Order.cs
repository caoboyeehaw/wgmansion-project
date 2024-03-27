using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WGMansion.MongoDB.Models;

namespace WGMansion.Api.Models.Ticker
{
    public class Order : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("type")]
        public string Type { get; set; } = "order";
        [BsonElement("active")]
        public bool Active { get; set; } = true;
        [BsonElement("ownerId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OwnerId { get; set; }
        [BsonElement("orderType")]
        public OrderType OrderType { get; set; }
        [BsonElement("symbol")]
        public string Symbol { get; set; }
        [BsonElement("price")]
        public float Price { get; set; }
        [BsonElement("quantity")]
        public int Quantity { get; set; }
        [BsonElement("maxQuantity")]
        public int MaxQuantity { get; set; }
        [BsonElement("postDate")]
        public DateTime PostDate { get; set; }
        [BsonElement("fulfillDate")]
        public DateTime FulfillDate { get; set; }
    }
}
