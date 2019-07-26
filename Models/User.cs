using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Player_Service.Models
{
    public class User {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        [JsonProperty("Name")]
        public string UserName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public int Purchases { get; set; }

        public int Level { get; set; }

        public string Device { get; set; }

        public BsonDocument Integrations { get; set; }
    }
}