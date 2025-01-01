using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages.Game;
using Quiz.Master.Hubs;
using Quiz.Master.Hubs.Models;
using RabbitMQ.Client;

namespace Quiz.Master.Consumers;

internal class MiniGameNotificationConsumer : ConsumerBase<MiniGameNotification>
{
    private readonly ISyncHubClient syncHubClient;

    public MiniGameNotificationConsumer(
        IConnection connection,
        ISyncHubClient syncHubClient,
        IQueueConsumerDefinition<MiniGameNotification> queueDefinition,
        ILogger<MiniGameNotificationConsumer> logger)
    : base(connection, queueDefinition, logger)
    {
        this.syncHubClient = syncHubClient;
    }

    protected override async Task ProcessMessageAsync(MiniGameNotification message, CancellationToken cancellationToken = default)
    {
        await syncHubClient.MiniGameNotification(new MiniGameNotificationSyncMessage(message.GameId, message.MiniGameType, message.Action, message.Metadata), cancellationToken);
    }
}