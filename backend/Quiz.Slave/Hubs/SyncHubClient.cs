using Microsoft.AspNetCore.SignalR;
using Quiz.Common.Hubs;
using Quiz.Slave.Hubs.Models;

namespace Quiz.Slave.Hubs;

internal class SyncHubClient : SyncHubClientBase<SyncHub>, ISyncHubClient
{
    public SyncHubClient(IHubContext<SyncHub> ctx, IHubConnection hubConnection) : base(ctx, hubConnection)
    {
    }

    public async Task GameStatusUpdated(GameStatusUpdateSyncMessage payload, CancellationToken cancellationToken = default)
        => await SendAsync(
            SyncDefinitions.SendGameStatusUpdate,
            payload,
            [],
            cancellationToken);

    public async Task MiniGameNotification(MiniGameNotificationSyncMessage payload, CancellationToken cancellationToken = default)
        => await SendAsync(
            SyncDefinitions.SendMiniGameNotification,
            payload, [],
            cancellationToken);
}