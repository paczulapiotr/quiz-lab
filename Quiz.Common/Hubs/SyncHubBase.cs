using Microsoft.AspNetCore.SignalR;

namespace Quiz.Common.Hubs;

public abstract class SyncHubBase : Hub
{
    private readonly IHubConnection _hubConnection;

    public SyncHubBase(IHubConnection hubConnection)
    {
        _hubConnection = hubConnection;
    }

    public override async Task OnConnectedAsync()
    {
        await _hubConnection.Connected(Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _hubConnection.Disconnected(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}