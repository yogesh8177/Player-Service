using OneSignal.AspNet.Core.SDK;
using OneSignal.AspNet.Core.SDK.Resources.Notifications;
using System.Collections.Generic;

namespace Player_Service.Services {
    public class OneSignalService {
        private readonly OneSignalClient client;

        public OneSignalService () {
            client = new OneSignalClient("");
        }

        public NotificationCreateResult createNotification (List<string> TargetSegments, string message) {
            var options = new NotificationCreateOptions();
            options.AppId = System.Guid.Parse("");
            options.IncludedSegments = TargetSegments;
            options.Contents.Add("Message", message);

            return client.Notifications.Create(options);
        }
    }
}