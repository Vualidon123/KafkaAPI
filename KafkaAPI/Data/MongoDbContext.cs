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

        public IMongoCollection<Event> Topics => _database.GetCollection<Event>("Event");
        public IMongoCollection<SubTopic> Subscribers => _database.GetCollection<SubTopic>("SubTopic");
    }

}
