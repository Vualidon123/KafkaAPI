using Confluent.Kafka.Admin;
using Confluent.Kafka;
using KafkaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KafkaAPI.Data;
using MongoDB.Driver;
using KafkaAPI.Producer;

namespace KafkaAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicCotroller : ControllerBase
    {
        private readonly string _kafkaServer;
        private readonly MongoDbContext _db;
        private readonly KafkaService _kafkaService;
        private readonly KProducer _producer;
        public TopicCotroller(IConfiguration config, MongoDbContext db,KafkaService kafkaService,KProducer producer)
        {
            _kafkaServer = config["Kafka:BootstrapServers"];
            _db = db;
            _kafkaService=kafkaService;
            _producer = producer;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateTopic([FromBody] CreateTopicRequest request)
        {
            var result = await _kafkaService.CreateTopicAsync(request.TopicName, request.NumPartitions, request.ReplicationFactor);
            return result ? Ok("Topic created") : BadRequest("Failed to create topic");
        }
        [HttpGet]
        public async Task<IActionResult> GetTopics()
        {
            var topics = await _kafkaService.ListTopicsAsync();
            return Ok(topics);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(string mes,string topic)
        {
           await _producer.ProduceAsync(topic,mes);
            return Ok();
        }
       
    }

}
