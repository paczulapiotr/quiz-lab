using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Broker.QueueDefinitions;
using RabbitMQ.Client;

namespace Quiz.Common.Broker.Builder;

public static class MessageBrokerBuilderExtensions
{
    public static async Task UseMessageBroker(this IApplicationBuilder app, CancellationToken cancellationToken = default)
    {
        var serviceProvider = app.ApplicationServices;
        var publisherDefinitions = serviceProvider.GetServices<IQueuePublisherDefinition>();
        var consumers = serviceProvider.GetServices<IConsumer>();
        var logger = serviceProvider.GetRequiredService<ILogger<IApplicationBuilder>>();

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var connection = serviceProvider.GetRequiredService<IConnection>();
                using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
                foreach (var def in publisherDefinitions)
                {
                    await def.RegisterPublisherAsync(channel, cancellationToken: cancellationToken);
                }

                foreach (var consumer in consumers)
                {
                    await consumer.RegisterAsync(cancellationToken);
                }
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while setting up the message broker.");
                await Task.Delay(5000, cancellationToken);
            }
        }

        cancellationToken.ThrowIfCancellationRequested();
    }
    public static void AddMessageBroker(this IServiceCollection services, string connectionString, Action<QueueConfig> configure)
    {
        services.AddSingleton(new ConnectionFactory() { 
            Uri = new Uri(connectionString), 
            ClientProvidedName = DeviceIdHelper.DeviceUniqueId 
        });
        services.AddSingleton<IConnection>(sp =>
        {
            var factory = sp.GetRequiredService<ConnectionFactory>();
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });

        // Configure queue definitions and consumers
        var config = new QueueConfig(services);
        configure(config);

        services.AddSingleton<IQueuePublisherDefinitionProvider, QueuePublisherDefinitionProvider>();
        services.AddSingleton<IPublisher, RabbitMQPublisher>();
    }
}