using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Quiz.Common;
using Quiz.Common.Broker.Builder;
using Quiz.Common.Messages.Game;
using Quiz.Common.Messages.Round;
using Quiz.Common.WebApplication;
using Quiz.Master;
using Quiz.Master.Consumers;
using Quiz.Master.Features.Game.CreateGame;
using Quiz.Master.Features.Game.GetGame;
using Quiz.Master.Features.Game.JoinGame;
using Quiz.Master.Persistance;

var builder = WebApplication.CreateSlimBuilder(args);
DeviceIdHelper.Setup(builder.Configuration["DeviceId"]);
builder.Services.AddMvcCore();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});
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
    opts.AddQueryHandler<GetGameHandler, GetGameQuery, GetGameResult>();
});
builder.Services.AddHostedService<ConsumerHostedService>();
builder.Services.AddHostedService<GameEngineHostedService>();

builder.Services
    .AddMessageBroker(
        builder.Configuration.GetConnectionString("RabbitMq")!,
        AppJsonSerializerContext.Default,
        opts =>
        {
            var deviceId = DeviceIdHelper.DeviceUniqueId;
            opts.AddPublisher(GameCreatedDefinition.Publisher());
            opts.AddPublisher(PlayerJoinedDefinition.Publisher());
            opts.AddPublisher(GameStartingDefinition.Publisher());
            opts.AddPublisher(GameStartedDefinition.Publisher());
            opts.AddPublisher(GameEndDefinition.Publisher());
            opts.AddPublisher(RoundEndDefinition.Publisher());
            opts.AddPublisher(RulesExplainedDefinition.Publisher());

            opts.AddOneTimeConsumer(GameStartedDefinition.Consumer(deviceId));
            opts.AddOneTimeConsumer(RoundEndedDefinition.Consumer());
            opts.AddPublisher(RulesExplainDefinition.Publisher());
            opts.AddOneTimeConsumer(RulesExplainedDefinition.Consumer(deviceId));
            opts.AddPublisher(RoundStartDefinition.Publisher());
            opts.AddOneTimeConsumer(RoundStartedDefinition.Consumer());
            opts.AddConsumer<GameStartingConsumer, GameStarting>(GameStartingDefinition.Consumer(deviceId));
            opts.AddConsumer<RulesExplainConsumer, RulesExplain>(RulesExplainDefinition.Consumer(deviceId));

        });

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(opts =>
    {
        opts.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddQuizServices();

var app = builder.Build();

//Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<QuizDbContext>();
    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

app.UseQuizCommonServices();
await app.UseMessageBroker();
app.MapEndpoints();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();


// Message Broker messages
[JsonSerializable(typeof(JoinGameRequest))]
[JsonSerializable(typeof(GameCreated))]
[JsonSerializable(typeof(PlayerJoined))]
[JsonSerializable(typeof(GameStarting))]
[JsonSerializable(typeof(GameStarted))]
[JsonSerializable(typeof(RoundEnd))]
[JsonSerializable(typeof(RoundEnded))]
[JsonSerializable(typeof(RoundStart))]
[JsonSerializable(typeof(RoundStarted))]
[JsonSerializable(typeof(GameEnd))]
[JsonSerializable(typeof(RulesExplain))]
[JsonSerializable(typeof(RulesExplained))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}