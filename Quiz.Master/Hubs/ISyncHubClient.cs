
using Quiz.Master.Hubs.Models;

namespace Quiz.Master.Hubs;

internal interface ISyncHubClient
{
    Task GameStatusUpdated(GameStatusUpdateSyncMessage payload, CancellationToken cancellationToken = default);

}