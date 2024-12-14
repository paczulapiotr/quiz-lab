using Quiz.Common.Broker.Messages;

namespace Quiz.Common.Broker.Consumer;

public interface IConsumer : IDisposable
{
    Task ConsumeAsync(CancellationToken cancellationToken = default);
}

public interface IOneTimeConsumer<TMessage> where TMessage : IMessage
{
    Task<TMessage?> ConsumeFirstAsync(Func<TMessage, CancellationToken, Task>? callback = null, Func<TMessage, bool>? condition = null, CancellationToken cancellationToken = default);
}


public interface IConsumer<TMessage> : IOneTimeConsumer<TMessage>, IConsumer where TMessage : IMessage
{
}