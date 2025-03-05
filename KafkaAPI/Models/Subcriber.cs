using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace KafkaAPI.Models
{
    public class SubTopic
    {
        
        public ObjectId Id { get; set; }
        [BsonElement("CallbackUrl")]
        public string CallbackUrl { get; set; }
        public string TopicName{ get; set; }
        public string subTopic { get; set; }
    }
}
