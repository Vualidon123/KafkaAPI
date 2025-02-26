namespace KafkaAPI.Consumer
{
    public class KafkaConsumerService : IHostedService
    {
        private readonly IKafkaConsumer _consumer;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private Task _consumerTask;

        public KafkaConsumerService(IKafkaConsumer consumer)
        {
            _consumer = consumer;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumerTask = Task.Run(() => _consumer.ConsumeAsync("your-topic", _cancellationTokenSource.Token), cancellationToken);
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            if (_consumerTask != null)
            {
                await _consumerTask;
            }
        }
    }
}
