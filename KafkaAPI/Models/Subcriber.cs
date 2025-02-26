using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace KafkaAPI.Models
{
    public class Subscriber
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int SubCrisberId {get;set; }

        [BsonElement("CallbackUrl")]
        public string CallbackUrl { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }
        public string TopicName{ get; set; }
    }
}
