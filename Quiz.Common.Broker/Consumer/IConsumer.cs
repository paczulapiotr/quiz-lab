using Quiz.Common.Broker.Messages;

namespace Quiz.Common.Broker.Consumer;


public interface IConsumer : IDisposable
{
    Task ConsumeAsync(CancellationToken cancellationToken = default);
}

public interface IConsumer<TMessage> : IConsumer where TMessage : IMessage
{
}