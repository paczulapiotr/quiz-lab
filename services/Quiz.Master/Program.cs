using Microsoft.EntityFrameworkCore;
using Quiz.Common;
using Quiz.Common.Broker.Builder;
using Quiz.Common.Messages.Game;
using Quiz.Common.WebApplication;
using Quiz.Master;
using Quiz.Master.Features.Game.CreateGame;
using Quiz.Master.Features.Game.GetGame;
using Quiz.Master.Features.Game.JoinGame;
using Quiz.Master.Hubs;
using Quiz.Master.Persistance;
using Quiz.Master.Consumers;
using Quiz.Master.Migrations;
using Quiz.Master.Features.MiniGame.GetMiniGame;
using Quiz.Master.Features.MiniGame.SendPlayerInteraction;

var builder = WebApplication.CreateSlimBuilder(args);
DeviceIdHelper.Setup(builder.Configuration["DeviceId"]);
builder.Services.AddMvcCore();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Quiz.Master", Version = "v1" });
});

// Configure SQLite DbContext
builder.Services.AddDbContextFactory<QuizDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLite")));
builder.Services.AddQuizCommonServices(opts =>
{
    opts.AddCommandHandler<JoinGameHandler, JoinGameCommand>();
    opts.AddCommandHandler<CreateGameHandler, CreateGameCommand>();
    opts.AddCommandHandler<SendPlayerInteractionHandler, SendPlayerInteractionCommand>();
    opts.AddQueryHandler<GetGameHandler, GetGameQuery, GetGameResult>();
    opts.AddQueryHandler<GetMiniGameHandler, GetMiniGameQuery, GetMiniGameResult>();

});
builder.Services.AddHostedService<ConsumerHostedService>();
builder.Services.AddHostedService<GameEngineHostedService>();

builder.Services
    .AddMessageBroker(
        builder.Configuration.GetConnectionString("RabbitMq")!,
        new SystemJsonSerializer(),
        opts =>
        {
            var deviceId = DeviceIdHelper.DeviceUniqueId;
            opts.AddPublisher(new GameStatusUpdateDefinition());
            opts.AddPublisher(new MiniGameUpdateDefinition());
            opts.AddPublisher(new MiniGameNotificationDefinition());
            opts.AddPublisher(new PlayerInteractionDefinition());
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
        opts.WithOrigins([builder.Configuration["Cors"]!])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddQuizServices();
builder.Services.AddQuizHub<SyncHub, ISyncHubClient, SyncHubClient>();

var app = builder.Build();

//Apply migrations on startup
await app.MigrateDatabaseAsync();
app.UseQuizCommonServices();
await app.UseMessageBroker();
app.MapEndpoints();
app.MapHub<SyncHub>("/sync").RequireCors("SignalR");
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();

