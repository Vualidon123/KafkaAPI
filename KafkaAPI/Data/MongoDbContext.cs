using KafkaAPI.Models;
using MongoDB.Driver;

namespace KafkaAPI.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            _database = client.GetDatabase(config["MongoDB:DatabaseName"]);
        }

        public IMongoCollection<Topic> Topics => _database.GetCollection<Topic>("Topics");
        public IMongoCollection<Subscriber> Subscribers => _database.GetCollection<Subscriber>("Subscribers");
    }

}
