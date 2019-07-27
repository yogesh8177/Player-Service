using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Player_Service.Models
{
    public class SendBirdGroupRequest {
        public string name { get; set; }

        public string custom_type { get; set; }

        public string inviter_id { get; set; }

        public List<string> user_ids { get; set; }
    }
}