public class SubscriberService
{
    private readonly Dictionary<string, List<string>> _subscribers = new();

    public void RegisterSubscriber(string topic, string callbackUrl)
    {
        if (!_subscribers.ContainsKey(topic))
        {
            _subscribers[topic] = new List<string>();
        }
        _subscribers[topic].Add(callbackUrl);
        Console.WriteLine($"Subscriber {callbackUrl} added to topic {topic}");
    }

    public List<string> GetSubscribers(string topic) => _subscribers.ContainsKey(topic) ? _subscribers[topic] : new List<string>();
}
