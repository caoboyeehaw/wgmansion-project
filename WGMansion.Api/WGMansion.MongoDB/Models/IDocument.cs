using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WGMansion.MongoDB.Models
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("active")]
        public bool Active { get; set; }
    }
}
