using Quiz.Common.Broker.Messages;
using RabbitMQ.Client;

namespace Quiz.Common.Broker.QueueDefinitions;

public interface IQueueExchange
{
    public Type MessageType { get; }
    public string ExchangeName { get; }
}

public interface IQueuePublisherDefinition : IQueueExchange
{
    public ExchangeType ExchangeType { get; }
    public string MapRoutingKey(string? routingKey = null);
    Task RegisterPublisherAsync(IChannel channel, CancellationToken cancellationToken = default);
}

public interface IQueuePublisherDefinition<TMessage> : IQueuePublisherDefinition where TMessage : IMessage { }


public interface IQueueConsumerDefinition : IQueueExchange
{
    public string QueueName { get; }
    Task RegisterConsumerAsync(IChannel channel, string? routingKey = null, CancellationToken cancellationToken = default);
}

public interface IQueueConsumerDefinition<TMessage> : IQueueConsumerDefinition where TMessage : IMessage { }


public interface IQueueDefinition<TMessage>
: IQueuePublisherDefinition<TMessage>, IQueueConsumerDefinition<TMessage> where TMessage : IMessage
{ }