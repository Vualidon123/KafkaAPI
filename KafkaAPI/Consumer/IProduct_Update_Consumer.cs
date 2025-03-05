using Confluent.Kafka;
using KafkaAPI.Data;
using KafkaAPI.Models;
using MongoDB.Driver;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public interface IProduct_Update_Consumer
{
    Task ConsumeAsync(string topic, CancellationToken cancellationToken);
}

public class Product_Update_Consumer : IProduct_Update_Consumer
{
    private readonly IConsumer<Null, string> _consumer;
    private readonly HttpClient _httpClient = new();
    private readonly MongoDbContext _context;
    private readonly IProducer<Null, string> _producer;
    public Product_Update_Consumer(MongoDbContext context)
    {
        _context=context;
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _consumer = new ConsumerBuilder<Null, string>(config).Build();
        var producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
        _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
    }

    public async Task ConsumeAsync(string topic, CancellationToken cancellationToken)
    {
        
        _consumer.Subscribe(topic);
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
               
                var consumeResult = _consumer.Consume(cancellationToken);
                    var message = consumeResult.Message.Value;
                Console.WriteLine($"Consumed message '{message}' at: '{consumeResult.TopicPartitionOffset}'.");
                // Fetch all registered subscribers for this topic
                var subscribers = await _context.Subscribers
                    .Find(sub => sub.TopicName == topic)
                    .ToListAsync();

                foreach (var subscriber in subscribers)
                {                  
                    await PushMessageToSubTopic(subscriber.subTopic, message);//gui tin nhan den subTopic
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Ensure the consumer leaves the group cleanly and final offsets are committed.
            _consumer.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private async Task PushMessageToSubTopic(string subTopic, string message)
    {
        try
        {
            var result = await _producer.ProduceAsync(subTopic, new Message<Null, string> { Value = message });
            Console.WriteLine($"Message '{message}' pushed to sub-topic '{subTopic}' at: '{result.TopicPartitionOffset}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to push message to sub-topic '{subTopic}': {ex.Message}");
        }
    }
}