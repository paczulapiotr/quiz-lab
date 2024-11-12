using Quiz.Common.Broker.Builder;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages;
using Quiz.Common.Messages.PingPong;
using Quiz.Slave.Consumers;
using Quiz.Slave.Hubs;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddSingleton<JsonSerializerContext>(AppJsonSerializerContext.Default);
builder.Services
    .AddMessageBroker(builder.Configuration.GetConnectionString("RabbitMq")!, opts =>
    {
        opts.AddDefinition<Ping, PingQueueDefinition>();
        opts.AddDefinition<Pong, PongQueueDefinition>();
        opts.AddConsumer<PingConsumer>();
        opts.AddConsumer<PongConsumer>();
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
app.MapGet("/ping", async (IPublisher publisher, CancellationToken cancellationToken = default) =>
{
    var message = new Ping("Hello, World!");
    await publisher.PublishAsync(message, cancellationToken);
    return Results.Ok("Ping message sent");
});

// Map the PingPongHub
app.MapHub<PingPongHub>("/pingpong");
await MessageBrokerBuilder.Invoke(app.Services);

app.Run();


public record Health(string? Status, DateTime? Timestamp = null, bool IsHealthy = true);

[JsonSerializable(typeof(Health))]
[JsonSerializable(typeof(Ping))]
[JsonSerializable(typeof(Pong))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
