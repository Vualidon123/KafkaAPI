using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaAPI.Data;
using KafkaAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Runtime.InteropServices;
using static Confluent.Kafka.ConfigPropertyNames;

public class KafkaService
{
    private readonly AdminClientConfig _adminConfig;
    private readonly MongoDbContext _context;
    private readonly IConsumer<Null, string> _consumer;
    public KafkaService(IConfiguration configuration,MongoDbContext context)
    {
        
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _consumer = new ConsumerBuilder<Null, string>(config).Build();
        _context = context;
        _adminConfig = new AdminClientConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"]
        };
    }

    public async Task<bool> CreateTopicAsync(string topicName, int numPartitions, short replicationFactor)
    {
        using var adminClient = new AdminClientBuilder(_adminConfig).Build();
        try
        {
            await adminClient.CreateTopicsAsync(new[]
            {
                new TopicSpecification { Name = topicName, NumPartitions = numPartitions, ReplicationFactor = replicationFactor }
            });
            var topic = new Event
            {
                Id = ObjectId.GenerateNewId(),
                Name = topicName,
            };
            _context.Topics.InsertOne(topic);
            return true;
        }
        catch (CreateTopicsException ex)
        {
            Console.WriteLine($"Error creating topic: {ex.Results[0].Error.Reason}");
            return false;
        }
    }

    public async Task<List<string>> ListTopicsAsync()
    {
        var topics = await _context.Topics.Find(_ => true).ToListAsync();
        return topics.Select(t => t.Name).ToList();
    }
    public  async Task<long> GetRemainingMessages(string topic)
    {
        var partition = new TopicPartition(topic, new Partition(0));
        var watermarkOffsets = _consumer.QueryWatermarkOffsets(partition, TimeSpan.FromSeconds(1));
        var currentOffset = _consumer.Position(partition);
        return watermarkOffsets.High.Value - currentOffset.Value;
    }
}
