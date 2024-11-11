using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Quiz.CommonLib.MessageBroker.Filter;
using RabbitMQ.Client;

namespace Quiz.CommonLib.MessageBroker;

public class MessageBrokerConfig
{
    private readonly IServiceCollection _services;

    public MessageBrokerConfig(IServiceCollection services)
    {
        _services = services;
    }

    public MessageBrokerConfig AddFanout<TMessage>(string exchangeName, string queueName)
    {
        _services.AddSingleton<IStartupFilter>(new MessageBrokerStartupFilter(exchangeName, queueName, ExchangeType.Fanout));
        return this;
    }

    public MessageBrokerConfig AddTopic<TMessage>(string exchangeName, string queueName, string routingKey)
    {
        _services.AddSingleton<IStartupFilter>(new MessageBrokerStartupFilter(exchangeName, queueName, ExchangeType.Topic, routingKey));
        return this;
    }

    public MessageBrokerConfig AddQueueToExchange(string exchangeName, string queueName, string exchangeType, string routingKey = "")
    {
        _services.AddSingleton<IStartupFilter>(new MessageBrokerStartupFilter(exchangeName, queueName, exchangeType, routingKey));
        return this;
    }
}