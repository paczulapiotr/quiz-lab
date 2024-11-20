
using System.Text.Json.Serialization;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages.PingPong;
using RabbitMQ.Client;

namespace Quiz.Slave.Consumers;

public class PongConsumer : ConsumerBase<Pong>
{
    public PongConsumer(IConnection connection, IQueueConsumerDefinition<Pong> queueDefinition, ILogger<PongConsumer> logger, JsonSerializerContext jsonSerializerContext)
    : base(connection, queueDefinition, logger, jsonSerializerContext)
    {
    }

    protected override Task ProcessMessageAsync(Pong message, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}