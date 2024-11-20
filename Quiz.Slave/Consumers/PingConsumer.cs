using System.Text.Json.Serialization;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages.PingPong;
using RabbitMQ.Client;

namespace Quiz.Slave.Consumers;

public class PingConsumer : ConsumerBase<Ping>
{
    private readonly IPublisher _publisher;

    public PingConsumer(
        IQueueConsumerDefinition<Ping> queueDefinition,
        IConnection connection,
        ILogger<PingConsumer> logger,
        JsonSerializerContext jsonSerializerContext,
        IPublisher publisher)
    : base(connection, queueDefinition, logger, jsonSerializerContext)
    {
        _publisher = publisher;
    }

    protected override async Task ProcessMessageAsync(Ping message, CancellationToken cancellationToken = default)
    {
        await _publisher.PublishAsync(new Pong("Hello, Pong!", message.CorrelationId), cancellationToken);
    }
}