using MongoDB.Bson.Serialization.Attributes;

namespace WGMansion.Api.Models.Ticker
{
    public class Stock
    {
        [BsonElement("symbol")]
        public string Symbol { get; set; }
        [BsonElement("averageprice")]
        public float AveragePrice { get; set; }
        [BsonElement("quantity")]
        public int Quantity { get; set; }
    }
}
