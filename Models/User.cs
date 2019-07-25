using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

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

        public int level { get; set; }

        public string device { get; set; }
    }
}