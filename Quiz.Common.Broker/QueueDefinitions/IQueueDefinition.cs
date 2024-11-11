using Quiz.Common.Broker.Messages;
using RabbitMQ.Client;

namespace Quiz.Common.Broker.QueueDefinitions;

public interface IQueueDefinition
{
    public string ExchangeName { get; }
    public string QueueName { get; }
    public string RoutingKey { get; }

    Task RegisterAsync(IChannel channel, CancellationToken cancellationToken = default);
}

public interface IQueueDefinition<TMessage> : IQueueDefinition where TMessage : IMessage
{ }