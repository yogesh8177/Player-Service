using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Player_Service.Models
{
    public class Message {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Text { get; set; }

        public string Sender { get; set; }

        public bool Inbox { get; set; }

        public bool Chat { get; set; }

        public bool Notification { get; set; }
    }
}