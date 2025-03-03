
using Microsoft.AspNetCore.SignalR;

namespace Quiz.Common.Hubs;

public abstract class SyncHubClientBase<THub>(IHubContext<THub> ctx, IHubConnection hubConnection) where THub : Hub
{
    private const int WaitForSeconds = 5;
    
    protected async Task SendAsync<TMessage>(string methodName, TMessage payload, IEnumerable<string> targetIds, CancellationToken cancellationToken = default)
    {
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        linkedCts.CancelAfter(TimeSpan.FromSeconds(WaitForSeconds));
        var linkedToken = linkedCts.Token;

        await Parallel.ForEachAsync(targetIds, linkedToken, async (targetId, token) =>
        {
            await hubConnection.WaitForConnection(targetId, token);
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