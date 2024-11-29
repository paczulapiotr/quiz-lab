using System.Text.Json.Serialization;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages.Round;
using Quiz.Slave.Hubs;
using RabbitMQ.Client;

namespace Quiz.Slave.Consumers;

internal class RoundStartConsumer : ConsumerBase<RoundStart>
{
    private readonly ISyncHubClient syncHubClient;

    public RoundStartConsumer(
        IConnection connection,
        ISyncHubClient syncHubClient,
        IQueueConsumerDefinition<RoundStart> queueDefinition,
        ILogger<RoundStartConsumer> logger,
        JsonSerializerContext jsonSerializerContext)
    : base(connection, queueDefinition, logger, jsonSerializerContext)
    {
        this.syncHubClient = syncHubClient;
    }

    protected override async Task ProcessMessageAsync(RoundStart message, CancellationToken cancellationToken = default)
    {
        await syncHubClient.RoundStart(message.GameId, cancellationToken);
    }
}