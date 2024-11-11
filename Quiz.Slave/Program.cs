using Quiz.Slave.Hubs;
using System.Text.Json.Serialization;
using Quiz.CommonLib.MessageBroker.Publisher;
using Quiz.CommonLib.MessageBroker.Builder;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddSingleton<JsonSerializerContext>(AppJsonSerializerContext.Default);
builder.Services
    .AddMessageBroker(builder.Configuration.GetConnectionString("RabbitMq")!)
    .AddMessages(opts =>
    {
        opts.AddFanout("quiz-exchange", "quiz-queue");
    });

// Register the GPIO hosted service with configuration
builder.Services.Configure<GpioSettings>(builder.Configuration.GetSection("GpioSettings"));
// builder.Services.AddHostedService<GpioHostedService>();
builder.Services.AddHostedService<ConsumerHostedService>();

// Add SignalR services
builder.Services.AddSignalR();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});


var app = builder.Build();

// Apply the CORS policy
app.UseCors();

app.MapGet("/health", () => new Health("Healthy", DateTime.Now));
app.MapGet("/publish", async (IPublisher publisher, JsonSerializerContext jsonSerializerContext) =>
{
    var message = new TestMessage("Hello, World!", 42, DateTime.Now);
    await publisher.PublishAsync(message);
    return Results.Ok();
});

// Map the PingPongHub
app.MapHub<PingPongHub>("/pingpong");
await MessageBrokerBuilder.Invoke(app.Services);

app.Run();


public record Health(string? Status, DateTime? Timestamp = null, bool IsHealthy = true);

[JsonSerializable(typeof(Health))]
[JsonSerializable(typeof(TestMessage))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
