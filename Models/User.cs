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

        public SendBirdIntegration SendBird { get; set; }

        public OneSignalIntegration OneSignal { get; set; }
    }

    public class SendBirdIntegration {
        public string AccessToken { get; set; }
        public string SystemChannelUrl { get; set; }
    }

    public class OneSignalIntegration {
        public string OneSignalId { get; set; }
    }
}