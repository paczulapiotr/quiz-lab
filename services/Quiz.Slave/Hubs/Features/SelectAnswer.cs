using Microsoft.AspNetCore.SignalR;
using Quiz.Slave.Hubs.Models;

namespace Quiz.Slave.Hubs;

internal partial class SyncHub
{
    public Task SelectAnswer(SelectAnswer answer)
    {
        _logger.LogInformation("SelectAnswer received from {ConnectionId}, answer: {answer}", Context.ConnectionId, answer);
        return Task.CompletedTask;
    }
}


internal static partial class SyncHubExtensions
{
    public static void SelectAnswer(this IHubContext<SyncHub> ctx, string answerId)
    {
        ctx.Clients.All.SendAsync("SelectAnswer", answerId);
    }
}