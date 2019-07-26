using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Player_Service.Models
{
    public class OneSignalRegisterPayload {
        public string app_id { get; set; }
        public int device_type { get; set; }
    }

    public enum OneSignalDevices {
        iOS = 0,
        ANDROID = 1 
    }
}