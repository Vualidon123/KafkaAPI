
using KafkaAPI.Data;
using KafkaAPI.Producer;

var builder = WebApplication.CreateBuilder(args);

// Thêm MongoDB Context
builder.Services.AddSingleton<MongoDbContext>();
/*builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();*/
builder.Services.AddSingleton<KafkaService>();
builder.Services.AddSingleton<SubscriberService>();
builder.Services.AddSingleton<KProducer>();
builder.Services.AddSingleton<IProduct_Update_Consumer, Product_Update_Consumer>();
builder.Services.AddSingleton<IProduct_Update_Consumer_sub1, Product_Update_Consumer_sub1>();
builder.Services.AddSingleton<IProduct_Update_Consumer_sub0, Product_Update_Consumer_sub0>();
builder.Services.AddHostedService<KafkaConsumerService>();
// Thêm Controllers
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