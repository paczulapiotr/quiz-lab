using RabbitMQ.Client;

namespace Quiz.CommonLib.MessageBroker.QueueDefinitions;

public interface IQueueDefinition
{
    Task RegisterAsync(IChannel channel, CancellationToken cancellationToken = default);
}