namespace Quiz.Common.Broker.Consumer;

public interface IConsumer : IDisposable
{
    Task ConsumeAsync(CancellationToken cancellationToken = default);
}