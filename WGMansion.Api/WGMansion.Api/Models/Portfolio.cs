using MongoDB.Bson.Serialization.Attributes;
using WGMansion.Api.Models.Ticker;

namespace WGMansion.Api.Models
{
    public class Portfolio
    {
        [BsonElement("money")]
        public float Money { get; set; }
        [BsonElement("stocks")]
        public List<Stock> Stocks { get; set; } = new List<Stock>();
    }
}
