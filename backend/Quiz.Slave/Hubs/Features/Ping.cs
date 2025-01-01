using Microsoft.AspNetCore.SignalR;

namespace Quiz.Slave.Hubs;

internal partial class SyncHub
{
    public async Task Ping()
    {
        await Clients.Caller.SendAsync("Pong");
    }
}