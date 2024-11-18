using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Broker.QueueDefinitions;
using RabbitMQ.Client;

namespace Quiz.Common.Broker.Builder;

public static class MessageBrokerBuilderExtensions
{
    public static async Task UseMessageBroker(this IApplicationBuilder app, CancellationToken cancellationToken = default)
    {
        var serviceProvider = app.ApplicationServices;
        var queueDefinitions = serviceProvider.GetServices<IQueueDefinition>();
        var connection = serviceProvider.GetRequiredService<IConnection>();
        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        foreach (var queueDefinition in queueDefinitions)
        {
            await queueDefinition.RegisterAsync(channel, cancellationToken);
        }
        cancellationToken.ThrowIfCancellationRequested();
    }
    public static void AddMessageBroker(this IServiceCollection services, string connectionString, JsonSerializerContext jsonSerializerContext, Action<QueueConfig> configure)
    {
        services.AddSingleton<JsonSerializerContext>(jsonSerializerContext);
        services.AddSingleton(new ConnectionFactory() { Uri = new Uri(connectionString) });
        services.AddSingleton(sp =>
        {
            var factory = sp.GetRequiredService<ConnectionFactory>();
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });

        // Configure queue definitions and consumers
        var config = new QueueConfig(services);
        configure(config);

        services.AddSingleton<IQueueDefinitionProvider, QueueDefinitionProvider>();
        services.AddSingleton<IPublisher, RabbitMQPublisher>();
    }
}