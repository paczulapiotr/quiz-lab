using System.Text.Json.Serialization;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages.Game;
using Quiz.Slave.Hubs;
using RabbitMQ.Client;

namespace Quiz.Slave.Consumers;

internal class GameEndConsumer : ConsumerBase<GameEnd>
{
    private readonly ISyncHubClient syncHubClient;

    public GameEndConsumer(
        IConnection connection,
        ISyncHubClient syncHubClient,
        IQueueConsumerDefinition<GameEnd> queueDefinition,
        ILogger<GameEndConsumer> logger,
        JsonSerializerContext jsonSerializerContext)
    : base(connection, queueDefinition, logger, jsonSerializerContext)
    {
        this.syncHubClient = syncHubClient;
    }

    protected override async Task ProcessMessageAsync(GameEnd message, CancellationToken cancellationToken = default)
    {
        await syncHubClient.GameEnd(message.GameId, cancellationToken);

    }
}