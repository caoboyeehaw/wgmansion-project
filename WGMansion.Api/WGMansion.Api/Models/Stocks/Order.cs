using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using WGMansion.MongoDB.Models;

namespace WGMansion.Api.Models.Stocks
{
    public class Order :IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("ownerId")]
        public ObjectId OwnerId { get; set; }
        [BsonElement("orderType")]
        public OrderType OrderType { get; set; }
        [BsonElement("price")]
        public int Price { get; set; }
        [BsonElement("quantity")]
        public int Quantity { get; set; }

    }
}
