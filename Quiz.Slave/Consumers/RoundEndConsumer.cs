using System.Text.Json.Serialization;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages.Round;
using Quiz.Slave.Hubs;
using RabbitMQ.Client;

namespace Quiz.Slave.Consumers;

internal class RoundEndConsumer : ConsumerBase<RoundEnd>
{
    private readonly ISyncHubClient syncHubClient;

    public RoundEndConsumer(
        IConnection connection,
        ISyncHubClient syncHubClient,
        IQueueConsumerDefinition<RoundEnd> queueDefinition,
        ILogger<RoundEndConsumer> logger,
        JsonSerializerContext jsonSerializerContext)
    : base(connection, queueDefinition, logger, jsonSerializerContext)
    {
        this.syncHubClient = syncHubClient;
    }

    protected override async Task ProcessMessageAsync(RoundEnd message, CancellationToken cancellationToken = default)
    {
        await syncHubClient.RoundEnd(message.GameId, cancellationToken);
    }
}