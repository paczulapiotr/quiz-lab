using Quiz.Common.Broker.Builder;
using Quiz.Common.Messages.PingPong;
using Quiz.Slave.Consumers;
using Quiz.Slave.Hubs;
using Quiz.Slave.Hubs.Models;
using Quiz.Slave.Commands;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Quiz.Common.WebApplication;

var builder = WebApplication.CreateSlimBuilder(args);
var rabbitConnectionString = builder.Configuration.GetConnectionString("RabbitMq")!;

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
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

// Add services
builder.Services
    .AddMessageBroker(
        rabbitConnectionString,
        AppJsonSerializerContext.Default,
        opts =>
        {
            opts.AddDefinition<Ping, PingQueueDefinition>();
            opts.AddDefinition<Pong, PongQueueDefinition>();
            opts.AddConsumer<PingConsumer>();
            opts.AddConsumer<PongConsumer>();
        });

builder.Services.AddQuizCommonServices(opts =>
{
    opts.AddCommandHandler<IPingCommandHandler, PingCommandHandler, PingCommand>();
});

builder.Services.AddScoped<ISyncHubClient, SyncHubClient>();

var app = builder.Build();
app.UseCors();
app.UseQuizCommonServices();
await app.UseMessageBroker();

// Map the PingPongHub
app.MapHub<SyncHub>("/sync");

// Map endpoints
app.MapPost("/select-answer/{answer}", async (string answer, ISyncHubClient syncClient, CancellationToken cancellationToken = default) =>
{
    await syncClient.SelectAnswer(new SelectAnswer(answer), cancellationToken);
    return Results.Ok("Answered");
});


app.MapPost("/ping", async (PingRequest req, IPingCommandHandler handler, CancellationToken cancellationToken = default) =>
{
    await handler.HandleAsync(new PingCommand(req.Message), cancellationToken);
    return Results.Ok("Ping message sent");
});

app.Run();

public record PingRequest(string Message);
public record Health(string? Status, DateTime? Timestamp = null, bool IsHealthy = true);

// HTTP REST messages
[JsonSerializable(typeof(Health))]
[JsonSerializable(typeof(PingRequest))]

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
