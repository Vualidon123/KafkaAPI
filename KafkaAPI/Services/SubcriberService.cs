using Confluent.Kafka;
using KafkaAPI.Data;
using KafkaAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;

public class SubscriberService
{
    private readonly MongoDbContext _context;
    private readonly AdminClientConfig _adminConfig;

    public SubscriberService(IConfiguration configuration ,MongoDbContext context)
    {
        _context = context;
        _adminConfig = new AdminClientConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"]
        };
    }
    public async Task RegisterSubscriberAsync(string topicName, string callbackUrl)
    {
        var topic = await _context.Topics.Find(sub => sub.Name == topicName).FirstOrDefaultAsync();//kiem tra topic co ton tai hay khong
        if (topic == null)
        {
            Console.WriteLine($"Topic {topicName} not found.");
            return;
        }

        var count = CountSubscribers(topicName);//lay so luon api subscriber
        var subTopicName = $"{topic.Name}_sub{count}";
        
        var subTopic = new SubTopic
        {
            Id = ObjectId.GenerateNewId(),
            TopicName = topicName,
            CallbackUrl = callbackUrl,
            subTopic = subTopicName
        };
        
        // Correctly update the SubTopics list using $push
        var filter = Builders<Event>.Filter.Eq(t => t.Name, topic.Name);
        var update = Builders<Event>.Update.Push(t => t.SubTopics, subTopic);

        await _context.Topics.UpdateOneAsync(filter, update);//cap nhap subtopic trong events

        // Add subscriber to a separate collection
        await _context.Subscribers.InsertOneAsync(subTopic);

        Console.WriteLine($"Subscriber {callbackUrl} added to topic {topicName}");
    }


    public List<SubTopic> GetSubscribers(string topic)
    {
        var subscribers = _context.Subscribers.Find(sub => sub.TopicName == topic).ToList();
        return subscribers;
    }
    public int CountSubscribers(string topicName)
    {
        var topic = _context.Topics.Find(sub => sub.Name == topicName).FirstOrDefault();
        if (topic != null)
        {
            return topic.SubTopics.Count;
        }
        return 0;
    }
}
