using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Player_Service.Models
{
    public class OneSignalRegistrationResponse {
        public string id { get; set; }
        public bool success { get; set; }
    }
}