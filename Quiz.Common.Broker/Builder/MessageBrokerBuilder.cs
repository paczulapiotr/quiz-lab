using Microsoft.Extensions.DependencyInjection;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Broker.QueueDefinitions;
using RabbitMQ.Client;

namespace Quiz.Common.Broker.Builder;

public class MessageBrokerBuilder
{
    public static async Task Invoke(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        var queueDefinitions = serviceProvider.GetServices<IQueueDefinition>();
        var connection = serviceProvider.GetRequiredService<IConnection>();
        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        foreach (var queueDefinition in queueDefinitions)
        {
            await queueDefinition.RegisterAsync(channel, cancellationToken);
        }
        cancellationToken.ThrowIfCancellationRequested();
    }
}

public static class MessageBrokerBuilderExtensions
{
    public static void AddMessageBroker(this IServiceCollection services, string connectionString, Action<QueueConfig> configure)
    {
        services.AddSingleton<IPublisher, RabbitMQPublisher>();
        services.AddSingleton(new ConnectionFactory() { Uri = new Uri(connectionString) });
        services.AddSingleton(sp =>
        {
            var factory = sp.GetRequiredService<ConnectionFactory>();
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });

        var config = new QueueConfig(services);
        configure(config);
    }
}