using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WGMansion.MongoDB.Models;

namespace WGMansion.Api.Models
{
    [BsonIgnoreExtraElements]
    public class Account : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("type")]
        public string Type { get; set; } = "account";
        [BsonElement("active")]
        public bool Active { get; set; } = true;
        [BsonElement("username")]
        public string UserName { get; set; }
        [BsonElement("password")]
        public string? Password { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("profilePictureId")]
        public string ProfilePictureId { get; set; }
        [BsonElement("role")]
        public string Role { get; set; }
        [BsonElement("creationDate")]
        public DateTime CreationDate { get; set; }
        [BsonElement("lastLogin")]
        public DateTime LastLogin { get; set; }
        [BsonIgnore]
        public string? Token { get; set; }
        [BsonElement("portfoilo")]
        public Portfolio Portfolio { get; set; } = new Portfolio();

    }
}
