
using Microsoft.AspNetCore.SignalR;

namespace Quiz.Common.Hubs;

public abstract class SyncHubClientBase<THub>(IHubContext<THub> ctx, IHubConnection hubConnection) where THub : Hub
{
    protected async Task SendAsync<TMessage>(string methodName, TMessage payload, IEnumerable<string> targetIds, CancellationToken cancellationToken = default)
    {
        await Parallel.ForEachAsync(targetIds, cancellationToken, async (targetId, token) =>
        {
            await hubConnection.WaitForConnection(targetId, cancellationToken);
            var connectionId = hubConnection.GetConnectionId(targetId);

            if (connectionId == null)
            {
                return;
            }

            await ctx.Clients.Client(connectionId)
                .SendAsync(
                    methodName,
                    payload,
                    token);
        });
    }

}