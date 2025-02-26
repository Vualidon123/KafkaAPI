using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace KafkaAPI.Models
{
    public class Topic
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        public int TopicId { get; set; }
        [BsonIgnore]
        public List<Subscriber> Subscribers { get; set; } = new List<Subscriber>();
    }
}
