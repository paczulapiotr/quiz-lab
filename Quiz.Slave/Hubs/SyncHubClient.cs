using Microsoft.AspNetCore.SignalR;
using Quiz.Slave.Hubs.Models;

namespace Quiz.Slave.Hubs;

internal class SyncHubClient(IHubContext<SyncHub> ctx) : ISyncHubClient
{
    public async Task SelectAnswer(SelectAnswer payload, CancellationToken cancellationToken = default)
    {
        await ctx.Clients.All.SendAsync(
            SyncDefinitions.SendSelectAnswer,
            payload,
            cancellationToken);
    }
}