﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
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
        public bool Active { get; set; }
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
    }
}
