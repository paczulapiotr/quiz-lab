using System.Text.Json.Serialization;
using Quiz.Common.Broker.Builder;

var builder = WebApplication.CreateSlimBuilder(args);


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

app.MapGet("/health", () => new Health("Healthy", DateTime.Now));

await MessageBrokerBuilder.Invoke(app.Services);
app.Run();

public record Health(string? Status, DateTime? Timestamp = null, bool IsHealthy = true);

[JsonSerializable(typeof(Health))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}