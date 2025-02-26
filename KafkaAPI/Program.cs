using KafkaAPI.Consumer;
using KafkaAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Th�m MongoDB Context
builder.Services.AddSingleton<MongoDbContext>();
/*builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();*/
builder.Services.AddSingleton<KafkaService>();
builder.Services.AddSingleton<SubscriberService>();
builder.Services.AddSingleton<IKafkaConsumer, KafkaConsumer>();
builder.Services.AddHostedService<KafkaConsumerService>();
// Th�m Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();