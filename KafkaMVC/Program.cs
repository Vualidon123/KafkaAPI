using KafkaAPI.Data;
using KafkaAPI.Producer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<MongoDbContext>();
/*builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();*/
builder.Services.AddSingleton<KafkaService>();
builder.Services.AddSingleton<SubscriberService>();
builder.Services.AddSingleton<KProducer>();
builder.Services.AddSingleton<IProduct_Update_Consumer, Product_Update_Consumer>();
builder.Services.AddSingleton<IProduct_Update_Consumer_sub1, Product_Update_Consumer_sub1>();
builder.Services.AddSingleton<IProduct_Update_Consumer_sub0, Product_Update_Consumer_sub0>();
builder.Services.AddHostedService<KafkaConsumerService>();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
