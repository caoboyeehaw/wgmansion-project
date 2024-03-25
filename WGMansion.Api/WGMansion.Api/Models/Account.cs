using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using WGMansion.MongoDB.Models;

namespace WGMansion.Api.Models
{
    public class Account : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("type")]
        public string Type { get; set; } = "account";
        [BsonElement("active")]
        public bool Active { get; set; }
        [BsonElement("username")]
        public string UserName { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("role")]
        public string Role { get; set; }
        [BsonIgnore]
        public string Token { get; set; }
        [BsonElement("portfoilo")]
        public Portfolio Portfolio { get; set; } = new Portfolio();

    }
}
