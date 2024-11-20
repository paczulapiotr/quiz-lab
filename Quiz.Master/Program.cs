using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Quiz.Common.Broker.Builder;
using Quiz.Common.Messages;
using Quiz.Common.WebApplication;
using Quiz.Master.Consumers;
using Quiz.Master.Persistance;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

// Configure SQLite DbContext
builder.Services.AddDbContextFactory<QuizDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLite")));
builder.Services.AddQuizCommonServices(opts => { });
builder.Services.AddHostedService<ConsumerHostedService>();
builder.Services
    .AddMessageBroker(
        builder.Configuration.GetConnectionString("RabbitMq")!,
        AppJsonSerializerContext.Default,
        opts =>
        {
            opts.AddConsumer<PlayerRegisterConsumer, PlayerRegister>(PlayerRegisterDefinition.Consumer());
            opts.AddPublisher(PlayerRegisteredDefinition.Publisher());

        });

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

//Apply migrations on startup
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
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
app.Run();

// Message Broker messages
[JsonSerializable(typeof(PlayerRegistered))]
[JsonSerializable(typeof(PlayerRegister))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}