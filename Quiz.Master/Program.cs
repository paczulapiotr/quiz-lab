using System.Text.Json.Serialization;
using Quiz.Common.Broker.Builder;
using Quiz.Common.Messages;
using Quiz.Common.WebApplication;
using Quiz.Master.Consumers;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});
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

app.UseQuizCommonServices();
await app.UseMessageBroker();
app.Run();

// Message Broker messages
[JsonSerializable(typeof(PlayerRegistered))]
[JsonSerializable(typeof(PlayerRegister))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}