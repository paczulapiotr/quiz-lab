using Microsoft.Extensions.Logging;
using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;
using RabbitMQ.Client;

namespace Quiz.Common.Broker.Consumer;

public abstract class ConsumerBase<TMessage> : OneTimeConsumer<TMessage>, IConsumer<TMessage>
where TMessage : class, IMessage
{
    private IChannel _channel = null!;
    private string _consumeTag = null!;

    public ConsumerBase(
        IConnection connection,
        IQueueConsumerDefinition<TMessage> queueDefinition,
        ILogger logger)
         : base(connection, queueDefinition, logger)
    {
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken = default)
    {
        if (_queueRouteName is null)
        {
            throw new InvalidOperationException("Queue route name is not set.");
        }

        _channel = await CreateChannelAsync();

        var consumer = await CreateAsyncConsumer(_channel, ProcessMessageAsync, cancellationToken: cancellationToken);

        _consumeTag = await _channel!.BasicConsumeAsync(_queueRouteName, false, consumer, cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
        {
            await CancelConsumptionAsync();
            _channel?.Dispose();
            _channel = null!;
        }
    }

    private async ValueTask CancelConsumptionAsync()
    {
        if (!string.IsNullOrEmpty(_consumeTag))
        {
            await _channel.BasicCancelAsync(_consumeTag);
            _consumeTag = null!;
        }
    }

    protected abstract Task ProcessMessageAsync(TMessage message, CancellationToken cancellationToken = default);

    public Task RegisterAsync(CancellationToken cancellationToken = default)
    {
        return base.RegisterAsync(cancellationToken: cancellationToken);
    }
}