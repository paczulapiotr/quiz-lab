
using Quiz.Master.Hubs.Models;

namespace Quiz.Master.Hubs;

internal interface ISyncHubClient
{
    Task MiniGameUpdated(MiniGameUpdateSyncMessage payload, CancellationToken cancellationToken = default);
    Task GameStatusUpdated(GameStatusUpdateSyncMessage payload, CancellationToken cancellationToken = default);
    Task MiniGameNotification(MiniGameNotificationSyncMessage payload, CancellationToken cancellationToken = default);
}