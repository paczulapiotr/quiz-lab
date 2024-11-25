using System.Text.Json.Serialization;
using Carter;
using Microsoft.EntityFrameworkCore;
using Quiz.Common.Broker.Builder;
using Quiz.Common.Messages;
using Quiz.Common.WebApplication;
using Quiz.Master;
using Quiz.Master.Consumers;
using Quiz.Master.Features.Game.JoinGame;
using Quiz.Master.Persistance;

var builder = WebApplication.CreateSlimBuilder(args);
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
    opts.AddQueryHandler<GetCurrentGameHandler, GetCurrentGameQuery, GetCurrentGameResult>();
});
builder.Services.AddHostedService<ConsumerHostedService>();
builder.Services
    .AddMessageBroker(
        builder.Configuration.GetConnectionString("RabbitMq")!,
        AppJsonSerializerContext.Default,
        opts =>
        {
            opts.AddConsumer<PlayerRegisterConsumer, PlayerRegister>(PlayerRegisterDefinition.Consumer());
            opts.AddPublisher(PlayerRegisteredDefinition.Publisher());
            opts.AddPublisher(GameCreatedDefinition.Publisher());
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
builder.Services.AddCarter();
builder.Services.AddQuizServices();

var app = builder.Build();

//Apply migrations on startup
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<QuizDbContext>();
    try
    {
        // dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

app.UseQuizCommonServices();
await app.UseMessageBroker();
app.UseCors();
app.MapCarter();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();


// Message Broker messages
[JsonSerializable(typeof(PlayerRegistered))]
[JsonSerializable(typeof(PlayerRegister))]
[JsonSerializable(typeof(JoinGameRequest))]
[JsonSerializable(typeof(GameCreated))]

internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}