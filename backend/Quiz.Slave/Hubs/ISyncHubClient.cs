using Quiz.Slave.Hubs.Models;

namespace Quiz.Slave.Hubs;

internal interface ISyncHubClient
{
    Task GameStatusUpdated(GameStatusUpdateSyncMessage payload, CancellationToken cancellationToken = default);
    Task MiniGameNotification(MiniGameNotificationSyncMessage payload, CancellationToken cancellationToken = default);
}