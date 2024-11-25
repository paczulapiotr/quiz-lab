using Microsoft.AspNetCore.SignalR;

namespace Quiz.Slave.Hubs;

internal partial class SyncHub : Hub
{
    private readonly ILogger<SyncHub> _logger;
    private readonly IHubConnection _hubConnection;

    public SyncHub(ILogger<SyncHub> logger, IHubConnection hubConnection)
    {
        _logger = logger;
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
