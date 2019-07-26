using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Player_Service.Models
{
    public class SendBirdUserPayload {
        public string user_id { get; set; }

        [BsonElement("nickname")]
        [JsonProperty("nickname")]
        public string nickname { get; set; }

        public string profile_url { get; set; }

        public bool issue_access_token { get; set; }
    }
}