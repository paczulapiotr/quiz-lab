namespace Quiz.CommonLib.MessageBroker.Publisher;

public interface IPublisher
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : IMessage;
}