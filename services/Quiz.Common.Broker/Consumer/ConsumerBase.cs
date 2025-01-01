using Microsoft.Extensions.Logging;
using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;
using RabbitMQ.Client;

namespace Quiz.Common.Broker.Consumer;

public abstract class ConsumerBase<TMessage> : OneTimeConsumer<TMessage>, IConsumer<TMessage>
where TMessage : class, IMessage
{
    public ConsumerBase(
        IConnection connection,
        IQueueConsumerDefinition<TMessage> queueDefinition,
        ILogger logger)
         : base(connection, queueDefinition, logger)
    {
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken = default)
    {
        await InitChannel(cancellationToken);

        var consumer = await CreateAsyncConsumer(ProcessMessageAsync, cancellationToken: cancellationToken);

        await _channel!.BasicConsumeAsync(_queueDefinition.QueueName, false, consumer, cancellationToken);
    }

    protected abstract Task ProcessMessageAsync(TMessage message, CancellationToken cancellationToken = default);
}