using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

public interface IKafkaConsumer
{
    Task ConsumeAsync(string topic, CancellationToken cancellationToken);
}

public class KafkaConsumer : IKafkaConsumer
{
    private readonly IConsumer<Null, string> _consumer;

    public KafkaConsumer()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:29092",
            GroupId = "consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _consumer = new ConsumerBuilder<Null, string>(config).Build();
    }

    public async Task ConsumeAsync(string topic, CancellationToken cancellationToken)
    {
        _consumer.Subscribe(topic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
            }
        }
        catch (OperationCanceledException)
        {
            // Ensure the consumer leaves the group cleanly and final offsets are committed.
            _consumer.Close();
        }
    }
}