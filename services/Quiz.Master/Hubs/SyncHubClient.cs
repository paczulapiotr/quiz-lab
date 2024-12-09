using Microsoft.AspNetCore.SignalR;
using Quiz.Master.Hubs.Models;

namespace Quiz.Master.Hubs;

internal class SyncHubClient(IHubContext<SyncHub> ctx, IHubConnection hubConnection) : ISyncHubClient
{
    private async Task SendAsync<TMessage>(string methodName, TMessage payload, CancellationToken cancellationToken = default)
    {
        await hubConnection.WaitForConnection(cancellationToken);
        await ctx.Clients.All.SendAsync(
            methodName,
            payload,
            cancellationToken);
    }

    public async Task GameStatusUpdated(GameStatusUpdateSyncMessage payload, CancellationToken cancellationToken = default)
        => await SendAsync(
            SyncDefinitions.SendGameStatusUpdate,
            payload,
            cancellationToken);
}