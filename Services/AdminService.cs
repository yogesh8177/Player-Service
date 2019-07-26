using Player_Service.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Player_Service.Services {
    public class AdminService {
        private readonly IMongoCollection<Admin> _admins;

        public AdminService(IPlayerServiceDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _admins = database.GetCollection<Admin>(settings.AdminsCollectionName);
        }
        public List<Admin> Get() =>
            _admins.Find(admin => true).ToList();

        public Admin Get(string id) =>
            _admins.Find<Admin>(admin => admin.Id == id).FirstOrDefault();

        public Admin Create(Admin admin)
        {
            _admins.InsertOne(admin);
            return admin;
        }

        public UpdateResult Update(string id, Admin adminIn) {
            var updateDef = Builders<Admin>.Update
                            .Set("UserName", adminIn.UserName)
                            .Set("Email", adminIn.Email)
                            .Set("Password", adminIn.Password);
            return _admins.UpdateOne(admin => admin.Id == id, updateDef);
        }

        public void Remove(string id) => 
            _admins.DeleteOne(admin => admin.Id == id);
    }
}

