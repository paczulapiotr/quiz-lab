using Microsoft.Extensions.DependencyInjection;

namespace Quiz.CommonLib.MessageBroker.Builder;

public class RabbitMQBuilder : IMessageBrokerBuilder
{
    public RabbitMQBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }
}