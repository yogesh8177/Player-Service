using Player_Service.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Player_Service.Services {
    public class UserService {
        private readonly IMongoCollection<User> _users;
        private readonly string sendBirdUrl = Environment.GetEnvironmentVariable("SENDBIRD_API_REQUEST_URL");
        private readonly string oneSignalUrl = Environment.GetEnvironmentVariable("ONESIGNAL_DEVICE_REGISTER_URL");
        public UserService(IPlayerServiceDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.PlayersCollectionName);
        }
        public List<User> Get() =>
            _users.Find(user => true).ToList();

        public User Get(string id) =>
            _users.Find<User>(user => user.Id == id).FirstOrDefault();

        public async Task<List<User>> GetUsersViaConditionsAsync(string conditions) {
            //var jsonObject = JsonConvert.DeserializeObject<User>(conditions);
            QueryParser parser = new QueryParser();
            // FilterDefinition<User> query = Builders<User>.Filter.Eq(user => user.Device, jsonObject.Device)
            //                                 & Builders<User>.Filter.Eq(user => user.Level, jsonObject.Level)
            //                                 & Builders<User>.Filter.Eq(user => user.Purchases, jsonObject.Purchases);
                                           
            // IAsyncCursor<User> usersCursor = await _users.FindAsync(filter: query, options: null);
            var query = parser.buildQuery(conditions);
            
            IAsyncCursor<User> usersCursor = await _users.FindAsync(filter: query, options: null);
            return await usersCursor.ToListAsync<User>();
        }
        public async Task<User> CreateAsync(User user)
        {
            _users.InsertOne(user);
            User sendBirdResponse = await this.registerSendBirdAsync(user);
            User oneSignalResponse = await this.registerOneSignalAsync(sendBirdResponse);
            this.Update(user.Id.ToString(), oneSignalResponse);
            
            return user;
        }

        private async Task<User> registerSendBirdAsync(User user) {
            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Api-Token", Environment.GetEnvironmentVariable("SENDBIRD_API_TOKEN"));

                var request = new SendBirdUserPayload();
                request.nickname = user.UserName;
                request.user_id = user.Id.ToString();
                request.profile_url = ""; // hard coding image url!!!
                request.issue_access_token = true;
        
                // string sendBirdUrl = Environment.GetEnvironmentVariable("SENDBIRD_API_REQUEST_URL") + "/v3/users";
                var sendbirdResponse = await client.PostAsJsonAsync<SendBirdUserPayload>(sendBirdUrl + "/v3/users", request);
                string Content = sendbirdResponse.Content.ReadAsStringAsync().Result;
                if (sendbirdResponse.IsSuccessStatusCode) {
                    var jsonResponse = JsonConvert.DeserializeObject<SendBirdRegistrationResponse>(Content);
                    string accessToken = jsonResponse.access_token;
                    string channelUrl = await this.createAdminChatChannelAsync(user);
                    user.Integrations = new MongoDB.Bson.BsonDocument {
                        { 
                            "sendBird", new MongoDB.Bson.BsonDocument {
                                { "access_token", accessToken },
                                { "system_channel_url", channelUrl }
                            } 
                        }
                    };
                }
                else {
                    // Lets not handle this for now
                }
            }
            return user;
        }

        private async Task<string> createAdminChatChannelAsync (User user) {
            string channelUrl = "";
            using (HttpClient client = new HttpClient()) {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Api-Token", Environment.GetEnvironmentVariable("SENDBIRD_API_TOKEN"));

                string inviterUserId = Environment.GetEnvironmentVariable("SYSTEM_ADMIN_USER_ID");
                var request = new SendBirdGroupRequest();
                request.name = "System Channel";
                request.custom_type = "System";
                request.inviter_id = inviterUserId;
                request.user_ids = new List<string>() { inviterUserId, user.Id.ToString() };
                
                var sendbirdResponse = await client.PostAsJsonAsync<SendBirdGroupRequest>(sendBirdUrl + "/v3/group_channels", request);

                string Content = sendbirdResponse.Content.ReadAsStringAsync().Result;
                if (sendbirdResponse.IsSuccessStatusCode) {
                    var jsonResponse = JsonConvert.DeserializeObject<SendBirdGroupResponse>(Content);
                    channelUrl = jsonResponse.channel_url;
                }
                else {
                    // Lets not handle this for now
                    Console.WriteLine("Error" + Content);
                }
            }
            return channelUrl;
        }

        private async Task<User> registerOneSignalAsync(User user) {
            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                OneSignalRegisterPayload oneSignalRequest = new OneSignalRegisterPayload();
                oneSignalRequest.app_id = Environment.GetEnvironmentVariable("ONESIGNAL_APP_ID");
                oneSignalRequest.device_type = user.Device == "android" ? (int) OneSignalDevices.ANDROID : (int) OneSignalDevices.iOS;
                var oneSignalResponse = await client.PostAsJsonAsync<OneSignalRegisterPayload>(oneSignalUrl, oneSignalRequest);
                string Content = "";
                if (oneSignalResponse.IsSuccessStatusCode) {
                    Content = oneSignalResponse.Content.ReadAsStringAsync().Result;
                    var jsonResponse = JsonConvert.DeserializeObject<OneSignalRegistrationResponse>(Content);
                    if (jsonResponse.success) {
                        string id = jsonResponse.id;
                        user.Integrations.Add("OneSignal", new MongoDB.Bson.BsonDocument { { "onesignal_id", id } });
                        this.Update(user.Id.ToString(), user);
                    }
                }
                else {
                    // Lets not handle this for now
                }
            }
            return user;
        }

        public UpdateResult Update(string id, User userIn) 
        {
            var updateDef = Builders<User>.Update
                            .Set("UserName", userIn.UserName)
                            .Set("Email", userIn.Email)
                            .Set("Level", userIn.Level)
                            .Set("Purchases", userIn.Purchases)
                            .Set("Device", userIn.Device)
                            .Set("Integrations", userIn.Integrations)
                            .Set("Password", userIn.Password);
            return _users.UpdateOne(user => user.Id == id, updateDef);
        }
        public void Remove(string id) => 
            _users.DeleteOne(user => user.Id == id);
    }
}

