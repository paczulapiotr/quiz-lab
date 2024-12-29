using Quiz.Common.Broker.Builder;
using Quiz.Slave.Consumers;
using Quiz.Slave.Hubs;
using System.Text.Json;
using Quiz.Common.WebApplication;
using Quiz.Common;
using Quiz.Slave;
using Quiz.Common.Messages.Game;

var builder = WebApplication.CreateSlimBuilder(args);
var rabbitConnectionString = builder.Configuration.GetConnectionString("RabbitMq")!;
DeviceIdHelper.Setup(builder.Configuration["DeviceId"]);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

// Register the GPIO hosted service with configuration
builder.Services.Configure<GpioSettings>(builder.Configuration.GetSection("GpioSettings"));
builder.Services.AddHostedService<GpioHostedService>();
builder.Services.AddHostedService<ConsumerHostedService>();

// Add SignalR
builder.Services.AddSignalR().AddJsonProtocol(options =>
{
    options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(opts =>
    {
        opts.WithOrigins(builder.Configuration["Cors"]!.Split(","))
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add services
builder.Services
    .AddMessageBroker(
        rabbitConnectionString,
        opts =>
        {
            var uniqueId = DeviceIdHelper.DeviceUniqueId;
            opts.AddConsumer<GameStatusUpdate, GameStatusUpdateConsumer>(new GameStatusUpdateDefinition().ToConsumer(uniqueId));
            opts.AddConsumer<MiniGameNotification, MiniGameNotificationConsumer>(new MiniGameNotificationDefinition().ToConsumer(uniqueId));
        });

builder.Services.AddQuizCommonServices(opts =>
{
    // opts.AddCommandHandler<ICommandHandler, CommandHandler, Command>();
});
builder.Services.AddQuizHub<SyncHub, ISyncHubClient, SyncHubClient>();

var app = builder.Build();
app.UseCors();
app.UseQuizCommonServices();
await app.UseMessageBroker();

// Map the PingPongHub
app.MapHub<SyncHub>("/sync");

// Map endpoints
app.MapEndpoints();
app.Run();
