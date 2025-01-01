using Quiz.Common.Broker.Consumer;
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
        ILogger<GameStatusUpdateConsumer> logger)
    : base(connection, queueDefinition, logger)
    {
        this.syncHubClient = syncHubClient;
    }

    protected override async Task ProcessMessageAsync(GameStatusUpdate message, CancellationToken cancellationToken = default)
    {
        await syncHubClient.GameStatusUpdated(new GameStatusUpdateSyncMessage(message.GameId, message.Status), cancellationToken);
    }
}