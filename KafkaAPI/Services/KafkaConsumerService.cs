using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

public class KafkaConsumerService : IHostedService
{
    private readonly IProduct_Update_Consumer _product_Update;
    private CancellationTokenSource _cancellationTokenSource;
    private readonly IProduct_Update_Consumer_sub1 _product_Update_sub;
    private readonly IProduct_Update_Consumer_sub0 _Update_Consumer_Sub0;

    public KafkaConsumerService(IProduct_Update_Consumer product_Update, IProduct_Update_Consumer_sub1 product_Update_sub1, IProduct_Update_Consumer_sub0 product_Update_Consumer_sub0)
    {
        _product_Update = product_Update;
        _product_Update_sub = product_Update_sub1;
        _Update_Consumer_Sub0 = product_Update_Consumer_sub0;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        Task.Run(() => _product_Update.ConsumeAsync("TEST1234", _cancellationTokenSource.Token), cancellationToken);
        Task.Run(() => _product_Update_sub.ConsumeAsync("TEST1234_sub0", _cancellationTokenSource.Token), cancellationToken);
        Task.Run(() => _Update_Consumer_Sub0.ConsumeAsync("TEST1234_sub1", _cancellationTokenSource.Token), cancellationToken);
        /*Task.Run(() => _product_Update_sub.ConsumeAsync("TEST1234_sub2", _cancellationTokenSource.Token), cancellationToken);
        Task.Run(() => _product_Update_sub.ConsumeAsync("TEST1234_sub3", _cancellationTokenSource.Token), cancellationToken);*/
        return Task.CompletedTask;
        }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
} 