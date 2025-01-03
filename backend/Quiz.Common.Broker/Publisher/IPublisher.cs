using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Broker.Publisher;

public interface IPublisher
{
    Task PublishAsync<T>(T message, string? routingKey = null, CancellationToken cancellationToken = default) where T : IMessage;
    Task PublishAsync<T>(T message, IQueuePublisherDefinition<T> queueDefinition, string? routingKey = null, CancellationToken cancellationToken = default) where T : IMessage;
}