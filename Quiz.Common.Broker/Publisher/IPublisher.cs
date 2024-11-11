using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Broker.Publisher;

public interface IPublisher
{
    Task PublishAsync<T>(T message, IQueueDefinition<T> queueDefinition, CancellationToken cancellationToken = default) where T : IMessage;
}