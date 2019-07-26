using Player_Service.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Player_Service.Services {
    public class UserService {
        private readonly IMongoCollection<User> _users;

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

        public User Create(User user)
        {
            _users.InsertOne(user);
            return user;
        }

        public UpdateResult Update(string id, User userIn) {
            var updateDef = Builders<User>.Update
                            .Set("UserName", userIn.UserName)
                            .Set("Email", userIn.Email)
                            .Set("Level", userIn.Level)
                            .Set("Purchases", userIn.Purchases)
                            .Set("Device", userIn.Device)
                            .Set("Password", userIn.Password);
            return _users.UpdateOne(user => user.Id == id, updateDef);
        }
           

        public void Remove(string id) => 
            _users.DeleteOne(user => user.Id == id);
    }
}

