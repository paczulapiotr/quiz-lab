using System.Text.Json.Serialization;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages.PingPong;
using RabbitMQ.Client;

namespace Quiz.Slave.Consumers;

public class PingConsumer : ConsumerBase<Ping>
{
    private readonly IQueueDefinition<Pong> pongQueueDefinition;
    private readonly IPublisher publisher;

    public PingConsumer(
        IQueueDefinition<Ping> queueDefinition,
        IConnection connection,
        ILogger<PingConsumer> logger,
        JsonSerializerContext jsonSerializerContext,
        IQueueDefinition<Pong> pongQueueDefinition,
        IPublisher publisher)
    : base(connection, queueDefinition, logger, jsonSerializerContext)
    {
        this.pongQueueDefinition = pongQueueDefinition;
        this.publisher = publisher;
    }

    protected override async Task ProcessMessageAsync(Ping message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Ping message: {message.Message}");
        await publisher.PublishAsync(new Pong("Hello, Pong!"), pongQueueDefinition, cancellationToken);
    }
}