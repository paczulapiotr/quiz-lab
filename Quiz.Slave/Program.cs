using Quiz.Common.Broker.Builder;
using Quiz.Slave.Consumers;
using Quiz.Slave.Hubs;
using Quiz.Slave.Hubs.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Quiz.Common.WebApplication;
using Quiz.Common;
using Quiz.Slave.ApiModels.Ping;
using Quiz.Slave;
using Quiz.Common.Messages.Game;

var builder = WebApplication.CreateSlimBuilder(args);
var rabbitConnectionString = builder.Configuration.GetConnectionString("RabbitMq")!;
DeviceIdHelper.Setup(builder.Configuration["DeviceId"]);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});
// Register the GPIO hosted service with configuration
builder.Services.Configure<GpioSettings>(builder.Configuration.GetSection("GpioSettings"));
builder.Services.AddHostedService<GpioHostedService>();
builder.Services.AddHostedService<ConsumerHostedService>();
// Add SignalR services with custom JsonSerializerOptions
builder.Services.AddSignalR().AddJsonProtocol(options =>
{
    options.PayloadSerializerOptions = new JsonSerializerOptions(AppJsonSerializerContext.Default.Options);
});
// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(opts =>
    {
        opts.WithOrigins([builder.Configuration["Cors"]!])
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
            var uniqueId = DeviceIdHelper.DeviceUniqueId;
            opts.AddConsumer<GameCreatedConsumer, GameCreated>(GameCreatedDefinition.Consumer(uniqueId));
            opts.AddConsumer<PlayerJoinedConsumer, PlayerJoined>(PlayerJoinedDefinition.Consumer(uniqueId));
            opts.AddConsumer<GameStatusUpdateConsumer, GameStatusUpdate>(GameStatusUpdateDefinition.Consumer(uniqueId));
        });

builder.Services.AddQuizCommonServices(opts =>
{
    // opts.AddCommandHandler<ICommandHandler, CommandHandler, Command>();
});

builder.Services.AddSingleton<IHubConnection, HubConnection>();
builder.Services.AddSingleton<ISyncHubClient, SyncHubClient>();

var app = builder.Build();
app.UseCors();
app.UseQuizCommonServices();
await app.UseMessageBroker();

// Map the PingPongHub
app.MapHub<SyncHub>("/sync");

// Map endpoints
app.MapEndpoints();

app.Run();


// HTTP REST messages
[JsonSerializable(typeof(PingRequest))]

// Message Broker messages
[JsonSerializable(typeof(GameCreated))]
[JsonSerializable(typeof(PlayerJoined))]
[JsonSerializable(typeof(GameStatusUpdate))]

// Hub messages
[JsonSerializable(typeof(SelectAnswer))]
[JsonSerializable(typeof(GameCreatedSyncMessage))]
[JsonSerializable(typeof(PlayerJoinedSyncMessage))]
[JsonSerializable(typeof(GameStatusUpdateSyncMessage))]

internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
