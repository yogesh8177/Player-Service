using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Player_Service.Models
{
    public class SendBirdGroupResponse {
        public string name { get; set; }
        public string custom_type { get; set; }
        public string channel_url { get; set; }
        public int member_count { get; set; }
        public string data { get; set; }
        public bool is_distinct { get; set; }
        public bool is_public { get; set; }
        public bool is_super { get; set; }
        public bool is_ephemeral { get; set; }
    }
}