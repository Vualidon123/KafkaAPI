namespace KafkaAPI.Models
{
    public class Request
    {
    }
    public class CreateTopicRequest
    {
        public string TopicName { get; set; }
        public int NumPartitions { get; set; } = 1;
        public short ReplicationFactor { get; set; } = 1;
    }
}
