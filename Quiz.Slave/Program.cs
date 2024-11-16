using Microsoft.AspNetCore.SignalR;
using Quiz.Common.Broker.Builder;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.PingPong;
using Quiz.Slave.Consumers;
using Quiz.Slave.Hubs;
using Quiz.Slave.Hubs.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services
    .AddMessageBroker(
        builder.Configuration.GetConnectionString("RabbitMq")!,
        AppJsonSerializerContext.Default,
        opts =>
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

// Add SignalR services with custom JsonSerializerOptions
builder.Services.AddSignalR().AddJsonProtocol(options =>
{
    options.PayloadSerializerOptions = new JsonSerializerOptions(AppJsonSerializerContext.Default.Options);
});

// Add services
builder.Services.AddScoped<ISyncHubClient, SyncHubClient>();

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
app.MapGet("/select-answer", async (string answer, ISyncHubClient syncClient, CancellationToken cancellationToken = default) =>
{
    await syncClient.SelectAnswer(new SelectAnswer(answer), cancellationToken);
    return Results.Ok("Answer selected");
});

// Map the PingPongHub
app.MapHub<SyncHub>("/sync");
await MessageBrokerBuilder.Invoke(app.Services);

app.Run();


public record Health(string? Status, DateTime? Timestamp = null, bool IsHealthy = true);

// HTTP REST messages
[JsonSerializable(typeof(Health))]
// Message Broker messages
[JsonSerializable(typeof(Ping))]
[JsonSerializable(typeof(Pong))]
// Hub messages
[JsonSerializable(typeof(PingHubMessage))]
[JsonSerializable(typeof(PongHubMessage))]
[JsonSerializable(typeof(SelectAnswer))]

internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
