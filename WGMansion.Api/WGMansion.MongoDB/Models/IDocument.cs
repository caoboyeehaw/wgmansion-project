using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WGMansion.MongoDB.Models
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        ObjectId Id { get; set; }
        [BsonElement("type")]
        string Type { get; set; }
    }
}
