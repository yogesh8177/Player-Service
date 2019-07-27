using Player_Service.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Player_Service.Services {
    public class MessageService {
        private readonly IMongoCollection<Message> _messages;
        private readonly SendBirdService _chatService;
        private readonly OneSignalService _notificationService;

        public MessageService(
            IPlayerServiceDatabaseSettings settings,
            SendBirdService chatService,
            OneSignalService notificationService
        )
        {
            this._chatService = chatService;
            this._notificationService = notificationService;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _messages = database.GetCollection<Message>(settings.AdminsCollectionName);
        }
        public List<Message> Get() =>
            _messages.Find(Message => true).ToList();

        public Message Get(string id) =>
            _messages.Find<Message>(Message => Message.Id == id).FirstOrDefault();

        public Message Create(Message Message)
        {
            _messages.InsertOne(Message);
            return Message;
        }

        public UpdateResult Update(string id, Message messageIn) {
            var updateDef = Builders<Message>.Update
                            .Set("Text", messageIn.Text)
                            .Set("Sender", messageIn.Sender)
                            .Set("Chat", messageIn.Chat)
                            .Set("Notification", messageIn.Notification);
            return _messages.UpdateOne(Message => Message.Id == id, updateDef);
        }

        public void Remove(string id) => 
            _messages.DeleteOne(Message => Message.Id == id);
    }
}

