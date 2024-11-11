using Microsoft.Extensions.DependencyInjection;
using Quiz.CommonLib.MessageBroker.QueueDefinitions;
using RabbitMQ.Client;

namespace Quiz.CommonLib.MessageBroker.Builder;

public class QueueConfig
{
    private readonly IServiceCollection _services;

    public QueueConfig(IServiceCollection services)
    {
        _services = services;
    }

    public QueueConfig AddFanout(string exchangeName, string queueName)
    {
        _services.AddSingleton<IQueueDefinition>(new QueueDefinition(exchangeName, queueName, ExchangeType.Fanout));
        return this;
    }

    public QueueConfig AddTopic(string exchangeName, string queueName, string routingKey)
    {
        _services.AddSingleton<IQueueDefinition>(new QueueDefinition(exchangeName, queueName, ExchangeType.Fanout, routingKey));
        return this;
    }

    public QueueConfig Add(string exchangeName, string queueName, string exchangeType, string routingKey = "")
    {
        _services.AddSingleton<IQueueDefinition>(new QueueDefinition(exchangeName, queueName, exchangeType, routingKey));
        return this;
    }
}