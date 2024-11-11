using Quiz.CommonLib.MessageBroker.Messages;

namespace Quiz.CommonLib.MessageBroker.Consumer;

public interface IConsumer<TMessage> where TMessage : IMessage
{
    Task ConsumeAsync(string queueName, CancellationToken cancellationToken = default);
}