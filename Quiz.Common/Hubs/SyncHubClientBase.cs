
using Microsoft.AspNetCore.SignalR;

namespace Quiz.Common.Hubs;

// public abstract class SyncHubClient<THub>(IHubContext<THub> ctx, IHubConnection hubConnection) where THub : Hub
// {
//     protected async Task SendAsync<TMessage>(string methodName, TMessage payload, CancellationToken cancellationToken = default)
//     {
//         await hubConnection.WaitForConnection(cancellationToken);
//         await ctx.Clients.All.SendAsync(
//             methodName,
//             payload,
//             cancellationToken);
//     }

// }