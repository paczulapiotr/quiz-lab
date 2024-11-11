using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Quiz.CommonLib.MessageBroker.Builder;

public static class MessageBrokerConfigurationExtensions
{
    public static IMessageBrokerBuilder AddQuizEvents(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(new ConnectionFactory() { Uri = new Uri(connectionString) });
        services.AddSingleton(sp =>
        {
            var factory = sp.GetRequiredService<ConnectionFactory>();
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });
        return new RabbitMQBuilder(services);
    }

    public static IMessageBrokerBuilder AddMessages(this IMessageBrokerBuilder builder, Action<MessageBrokerConfig> configure)
    {
        var config = new MessageBrokerConfig(builder.Services);
        configure(config);
        return builder;
    }
}