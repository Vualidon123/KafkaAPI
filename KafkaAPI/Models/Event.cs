using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace KafkaAPI.Models
{
    public class Event
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        public int TopicId { get; set; }
        
        public List<SubTopic> SubTopics { get; set; } = new List<SubTopic>();
    }
}
