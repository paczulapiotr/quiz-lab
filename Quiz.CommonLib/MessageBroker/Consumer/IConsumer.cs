using Quiz.CommonLib.MessageBroker;

public interface IConsumer<TMessage> where TMessage : IMessage
{
    Task ConsumeAsync(TMessage message, CancellationToken cancellationToken = default);
}