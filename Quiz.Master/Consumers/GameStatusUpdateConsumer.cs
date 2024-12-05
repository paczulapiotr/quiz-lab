using System.Text.Json.Serialization;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.JsonSerializer;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages.Game;
using Quiz.Master.Hubs;
using Quiz.Master.Hubs.Models;
using RabbitMQ.Client;

namespace Quiz.Master.Consumers;

internal class GameStatusUpdateConsumer : ConsumerBase<GameStatusUpdate>
{
    private readonly ISyncHubClient syncHubClient;

    public GameStatusUpdateConsumer(
        IConnection connection,
        ISyncHubClient syncHubClient,
        IQueueConsumerDefinition<GameStatusUpdate> queueDefinition,
        ILogger<GameStatusUpdateConsumer> logger,
        IJsonSerializer jsonSerializer)
    : base(connection, queueDefinition, logger, jsonSerializer)
    {
        this.syncHubClient = syncHubClient;
    }

    protected override async Task ProcessMessageAsync(GameStatusUpdate message, CancellationToken cancellationToken = default)
    {
        await syncHubClient.GameStatusUpdated(new GameStatusUpdateSyncMessage(message.GameId, message.Status), cancellationToken);
    }
}