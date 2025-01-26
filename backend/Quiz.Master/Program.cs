using Microsoft.EntityFrameworkCore;
using Carter;
using Quiz.Common;
using Quiz.Common.Broker.Builder;
using Quiz.Common.Messages.Game;
using Quiz.Common.WebApplication;
using Quiz.Master;
using Quiz.Storage;
using Quiz.Master.Hubs;
using Quiz.Master.Consumers;

var builder = WebApplication.CreateSlimBuilder(args);
DeviceIdHelper.Setup(builder.Configuration["DeviceId"]);
var connectionString = builder.Configuration["Mongo:ConnectionString"]!;
var databaseName = builder.Configuration["Mongo:Database"]!;
builder.Services.AddStorage(connectionString, databaseName);
builder.Services.AddCarter();
builder.Services.AddMvcCore();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Quiz.Master", Version = "v1" });
});
builder.Services.AddQuizCommonServices();
builder.Services.AddHostedService<ConsumerHostedService>();
builder.Services.AddHostedService<GameEngineHostedService>();
builder.Services
    .AddMessageBroker(
        builder.Configuration.GetConnectionString("RabbitMq")!,
        opts =>
        {
            var deviceId = DeviceIdHelper.DeviceUniqueId;
            opts.AddPublisher(new GameStatusUpdateDefinition());
            opts.AddPublisher(new MiniGameUpdateDefinition());
            opts.AddPublisher(new MiniGameNotificationDefinition());
            opts.AddPublisher(new PlayerInteractionDefinition());
            opts.AddPublisher(new NewGameCreationDefinition());
            opts.AddOneTimeConsumer(new NewGameCreationDefinition().ToConsumer(deviceId));
            opts.AddOneTimeConsumer(new GameStatusUpdateSingleDefinition().ToConsumer(deviceId + "-single"));
            opts.AddOneTimeConsumer(new MiniGameUpdateSingleDefinition().ToConsumer(deviceId + "-single"));
            opts.AddOneTimeConsumer(new PlayerInteractionDefinition().ToConsumer(deviceId + "-single"));
            opts.AddConsumer<MiniGameNotification, MiniGameNotificationConsumer>(new MiniGameNotificationDefinition().ToConsumer(deviceId));
            opts.AddConsumer<GameStatusUpdate, GameStatusUpdateConsumer>(new GameStatusUpdateDefinition().ToConsumer(deviceId));
        });
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(opts =>
    {
        opts.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
    options.AddPolicy("SignalR", opts =>
    {
        opts.WithOrigins(builder.Configuration["Cors"]!.Split(","))
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});
builder.Services.AddQuizServices(builder.Configuration);
builder.Services.AddQuizHub<SyncHub, ISyncHubClient, SyncHubClient>();

var app = builder.Build();
app.UseQuizCommonServices();
await app.UseMessageBroker();
app.MapCarter();
app.MapHub<SyncHub>("/sync").RequireCors("SignalR");
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();

