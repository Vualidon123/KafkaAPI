using KafkaAPI.Data;
using KafkaAPI.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/subscribers")]
[ApiController]
public class SubscriberController : ControllerBase
{
    private readonly SubscriberService _subscriberService;
    private readonly MongoDbContext _context;
    public SubscriberController(SubscriberService subscriberService,MongoDbContext context)
    {
        _subscriberService = subscriberService;
        _context = context;
    }

    [HttpPost("register")]
    public IActionResult RegisterSubscriber([FromBody] SubscribeRequest request)
    {
        _subscriberService.RegisterSubscriber(request.Topic, request.CallbackUrl);
        var subcri = new Subscriber
        {
            TopicName = request.Topic,
            CallbackUrl = request.CallbackUrl
        };
        _context.Subscribers.InsertOne(subcri);
        return Ok("Subscriber registered");
    }
    [HttpGet]
    public IActionResult GetSubcrisbers(string topic)
    {
        var subcribers=_subscriberService.GetSubscribers(topic);
        return Ok(subcribers);
    }
}

public class SubscribeRequest
{
    public string Topic { get; set; }
    public string CallbackUrl { get; set; }
}
