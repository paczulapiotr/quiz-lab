using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages.Game;
using Quiz.Slave.Hubs;
using Quiz.Slave.Hubs.Models;
using RabbitMQ.Client;

namespace Quiz.Slave.Consumers;

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