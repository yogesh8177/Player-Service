using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Player_Service.Services {
    public class SendBirdService {
        public SendBirdService () {
        }

        public async Task<bool> sendSystemChatMessageAsync (string channelUrl, string message) {
            using (HttpClient client = new HttpClient()) {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Api-Token", Environment.GetEnvironmentVariable("SENDBIRD_API_TOKEN"));

                var requestBody = new {
                    message_type = "ADMM",
                    message = message
                };

                var response = await client.PostAsJsonAsync(Environment.GetEnvironmentVariable("SENDBIRD_API_REQUEST_URL") + "/v3/group_channels/" + channelUrl + "/messages", requestBody);
                string Content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("SendBird response " + Content);
                if (response.IsSuccessStatusCode) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
    }
    
}