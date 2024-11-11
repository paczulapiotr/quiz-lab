using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quiz.CommonLib.MessageBroker.Publisher;
using Quiz.CommonLib.MessageBroker.QueueDefinitions;
using RabbitMQ.Client;

namespace Quiz.CommonLib.MessageBroker.Builder;

public class MessageBrokerBuilder
{
    private readonly IServiceCollection _services;

    public MessageBrokerBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public MessageBrokerBuilder AddMessages(Action<QueueConfig> configure)
    {
        var config = new QueueConfig(_services);
        configure(config);

        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var cancellationToken = cancellationTokenSource.Token;

        return this;
    }

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
    public static MessageBrokerBuilder AddMessageBroker(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IPublisher, RabbitMQPublisher>();
        services.AddSingleton(new ConnectionFactory() { Uri = new Uri(connectionString) });
        services.AddSingleton(sp =>
        {
            var factory = sp.GetRequiredService<ConnectionFactory>();
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });

        return new MessageBrokerBuilder(services);
    }
}