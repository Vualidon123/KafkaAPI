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
    public IActionResult RegisterSubscriber([FromBody] SubscribeRequest request)
    {
        _subscriberService.RegisterSubscriber(request.Topic, request.CallbackUrl);
        return Ok("Subscriber registered");
    }
}

public class SubscribeRequest
{
    public string Topic { get; set; }
    public string CallbackUrl { get; set; }
}
