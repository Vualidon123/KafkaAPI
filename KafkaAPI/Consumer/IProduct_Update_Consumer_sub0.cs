using Confluent.Kafka;
using KafkaAPI.Data;
using KafkaAPI.Models;
using MongoDB.Driver;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public interface IProduct_Update_Consumer_sub0
{
    Task ConsumeAsync(string topic, CancellationToken cancellationToken);
    
}


public class Product_Update_Consumer_sub0 : IProduct_Update_Consumer_sub0
{
    private readonly IConsumer<Null, string> _consumer;
    private readonly IProducer<Null, string> _producer;
    private readonly HttpClient _httpClient = new();
    private readonly MongoDbContext _context;

    public Product_Update_Consumer_sub0(MongoDbContext context)
    {
        _context = context;
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();

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
                var subscriber = await _context.Subscribers.Find(sub => sub.subTopic == topic).FirstOrDefaultAsync();//lay subscriber api
                Console.WriteLine($"Consumed message '{message}' at: '{consumeResult.TopicPartitionOffset}'.");
                /* var content = new StringContent(JsonSerializer.Serialize(new { Message = message }), System.Text.Encoding.UTF8, "application/json");
                 await _httpClient.PutAsync(subscriber.CallbackUrl, content);//gui message  den api       */
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
   
    
}
