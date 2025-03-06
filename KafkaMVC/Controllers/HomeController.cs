using KafkaAPI.Producer;
using KafkaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static Confluent.Kafka.ConfigPropertyNames;

namespace KafkaMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KafkaService _kafkaService;
        private readonly KProducer _producer;
        private readonly SubscriberService _subscriberService;

        public HomeController(ILogger<HomeController> logger, KafkaService kafkaService, KProducer producer, SubscriberService subscriberService)
        {
            _logger = logger;
            _kafkaService = kafkaService;
            _producer = producer;
            _subscriberService = subscriberService;
        }

        public async Task<IActionResult> Index(string topicName)
        {
            var topics = await _kafkaService.ListTopicsAsync();
            var subscribers =  _subscriberService.GetSubscribers(topicName);
           
            Console.WriteLine(subscribers);
            ViewBag.Subscribers = subscribers;
            return View(topics);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic(string topicName, int numPartitions, short replicationFactor)
        {
            var result = await _kafkaService.CreateTopicAsync(topicName, numPartitions, replicationFactor);
            return result ? RedirectToAction("Index") : BadRequest("Failed to create topic");
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(string message, string topic)
        {
            await _producer.ProduceAsync(topic, message);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterSubscriber(string topic, string callbackUrl)
        {
            await _subscriberService.RegisterSubscriberAsync(topic, callbackUrl);
            return Ok("Subscriber registered");
        }

        [HttpGet]
        public IActionResult GetSubscribers(string topic)
        {
            var subscribers = _subscriberService.GetSubscribers(topic);
            return Ok(subscribers);
        }
    }
}
