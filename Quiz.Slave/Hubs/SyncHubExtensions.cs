using Microsoft.AspNetCore.SignalR;

namespace Quiz.Slave.Hubs;

internal static class SyncHubExtensions
{
    public static void SelectAnswer(this IHubContext<SyncHub> ctx, string answerId)
    {
        ctx.Clients.All.SendAsync("SelectAnswer", answerId);
    }
}