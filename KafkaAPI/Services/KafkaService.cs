using Confluent.Kafka;
using Confluent.Kafka.Admin;

public class KafkaService
{
    private readonly AdminClientConfig _adminConfig;

    public KafkaService(IConfiguration configuration)
    {
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
        using var adminClient = new AdminClientBuilder(_adminConfig).Build();
        var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));
        return metadata.Topics.Select(t => t.Topic).ToList();
    }
}
