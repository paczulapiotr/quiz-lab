using Microsoft.AspNetCore.SignalR;
using Quiz.Common.Hubs;
using Quiz.Slave.Hubs.Models;

namespace Quiz.Slave.Hubs;

internal class SyncHubClient : SyncHubClientBase<SyncHub>, ISyncHubClient
{
    public SyncHubClient(IHubContext<SyncHub> ctx, IHubConnection hubConnection) : base(ctx, hubConnection)
    {
    }

    public async Task GameCreated(GameCreatedSyncMessage payload, CancellationToken cancellationToken = default)
        => await SendAsync(
            SyncDefinitions.SendGameCreated,
            payload,
            cancellationToken);

    public async Task SelectAnswer(SelectAnswer payload, CancellationToken cancellationToken = default)
        => await SendAsync(
            SyncDefinitions.SendSelectAnswer,
            payload,
            cancellationToken);

    public async Task PlayerJoined(PlayerJoinedSyncMessage payload, CancellationToken cancellationToken = default)
        => await SendAsync(
            SyncDefinitions.SendPlayerJoined,
            payload,
            cancellationToken);

    public async Task GameStatusUpdated(GameStatusUpdateSyncMessage payload, CancellationToken cancellationToken = default)
        => await SendAsync(
            SyncDefinitions.SendGameStatusUpdate,
            payload,
            cancellationToken);

    public async Task MiniGameUpdated(MiniGameUpdateSyncMessage payload, CancellationToken cancellationToken = default)
        => await SendAsync(
            SyncDefinitions.SendMiniGameUpdate,
            payload,
            cancellationToken);
}