using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Player_Service.Services {
    public class OneSignalService {
        public OneSignalService () {
        }

        public async Task<bool> createNotificationAsync (List<string> TargetPlayerIds, string message) {
            using (HttpClient client = new HttpClient()) {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestBody = new {
                    app_id = System.Guid.Parse(Environment.GetEnvironmentVariable("ONESIGNAL_APP_ID")),
                    include_player_ids = TargetPlayerIds.ToArray(),
                    data = new { tag = "message" },
                    contents = new { en = "Notification message here..." }
                };

                var response = await client.PostAsJsonAsync(Environment.GetEnvironmentVariable("ONESIGNAL_NOTIFICATION_CREATE_URL"), requestBody);
                string Content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Notification sent response " + Content);
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