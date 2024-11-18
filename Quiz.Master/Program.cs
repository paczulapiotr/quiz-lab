using System.Text.Json.Serialization;
using Quiz.Common.Broker.Builder;
using Quiz.Common.WebApplication;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddQuizCommonServices(opts => { });
builder.Services
    .AddMessageBroker(
        builder.Configuration.GetConnectionString("RabbitMq")!,
        AppJsonSerializerContext.Default,
        opts =>
        {
            // todo
        });

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.UseQuizCommonServices();
await app.UseMessageBroker();
app.Run();

[JsonSerializable(typeof(string))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}