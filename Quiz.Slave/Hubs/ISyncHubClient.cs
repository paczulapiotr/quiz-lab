using Quiz.Slave.Hubs.Models;

namespace Quiz.Slave.Hubs;

internal interface ISyncHubClient
{
    Task SelectAnswer(SelectAnswer payload, CancellationToken cancellationToken = default);
    Task GameCreated(GameCreatedSyncMessage payload, CancellationToken cancellationToken = default);
    Task PlayerJoined(PlayerJoinedSyncMessage payload, CancellationToken cancellationToken = default);
    Task GameStatusUpdated(GameStatusUpdateSyncMessage payload, CancellationToken cancellationToken = default);
}