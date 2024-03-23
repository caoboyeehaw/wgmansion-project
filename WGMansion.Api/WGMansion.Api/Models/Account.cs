using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using WGMansion.MongoDB.Models;

namespace WGMansion.Api.Models
{
    public class Account : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("username")]
        public string UserName { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("userRole")]
        public string UserRole { get; set; }
        public string Token { get; set; }
    }
}
