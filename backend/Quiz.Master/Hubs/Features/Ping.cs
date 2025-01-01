using Microsoft.AspNetCore.SignalR;

namespace Quiz.Master.Hubs;

internal partial class SyncHub
{
    public async Task Ping()
    {
        await Clients.Caller.SendAsync("Pong");
    }
}