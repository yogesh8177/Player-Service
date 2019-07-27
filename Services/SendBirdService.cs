using SendBird;
using WebSocketSharp;
using System.Collections.Generic;
using System;


namespace Player_Service.Services {
    public class SendBirdService {
        public SendBirdService () {
            SendBirdClient.Init(Environment.GetEnvironmentVariable("SENDBIRD_APP_ID"));
        }

        public void sendSystemChatMessage (string userId, string channelUrl, string message) {
            SendBirdClient.Connect(userId, (User user, SendBirdException connectException) => {
                // get channel
                GroupChannel.GetChannel(channelUrl,
                    (GroupChannel groupChannel, SendBirdException getChannelException) => {
                    // send message
                    groupChannel.SendUserMessage(message,
                        (UserMessage userMessage, SendBirdException sendMessageException) => {
                        // message sent
                        });
                });
            });
        }
    }
    
}