using KafkaAPI.Data;
using KafkaAPI.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/subscribers")]
[ApiController]
public class SubscriberController : ControllerBase
{
    private readonly SubscriberService _subscriberService;

    public SubscriberController(SubscriberService subscriberService)
    {
        _subscriberService = subscriberService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterSubscriber([FromBody] SubscribeRequest request)
    {
       await _subscriberService.RegisterSubscriberAsync(request.Topic, request.CallbackUrl);
        return Ok("Subscriber registered");
    }

    [HttpGet]
    public IActionResult GetSubcrisbers(string topic)
    {
        var subcribers = _subscriberService.GetSubscribers(topic);
        return Ok(subcribers);
    }
}

public class SubscribeRequest
{
    public string Topic { get; set; }
    public string CallbackUrl { get; set; }
}
