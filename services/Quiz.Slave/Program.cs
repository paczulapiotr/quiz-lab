using Quiz.Common.Broker.Builder;
using Quiz.Slave.Consumers;
using Quiz.Slave.Hubs;
using System.Text.Json;
using Quiz.Common.WebApplication;
using Quiz.Common;
using Quiz.Slave;
using Quiz.Common.Messages.Game;

var jsonSerializerOptions = new JsonSerializerOptions(AppJsonSerializerContext.Default.Options)
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

var builder = WebApplication.CreateSlimBuilder(args);
var rabbitConnectionString = builder.Configuration.GetConnectionString("RabbitMq")!;
DeviceIdHelper.Setup(builder.Configuration["DeviceId"]);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});
// Register the GPIO hosted service with configuration
builder.Services.Configure<GpioSettings>(builder.Configuration.GetSection("GpioSettings"));
builder.Services.AddHostedService<GpioHostedService>();
builder.Services.AddHostedService<ConsumerHostedService>();
// Add SignalR services with custom JsonSerializerOptions
builder.Services.AddSignalR().AddJsonProtocol(options =>
{
    options.PayloadSerializerOptions = jsonSerializerOptions;
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
        new AppJsonSerialization(jsonSerializerOptions),
        opts =>
        {
            var uniqueId = DeviceIdHelper.DeviceUniqueId;
            opts.AddConsumer<GameStatusUpdate, GameStatusUpdateConsumer>(new GameStatusUpdateDefinition().ToConsumer(uniqueId));
            opts.AddConsumer<MiniGameUpdate, MiniGameUpdateConsumer>(new MiniGameUpdateDefinition().ToConsumer(uniqueId));

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
