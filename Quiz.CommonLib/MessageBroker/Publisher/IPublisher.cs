using Quiz.CommonLib.Messages;

namespace Quiz.CommonLib.Publisher;

public interface IPublisher
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : IMessage;
}