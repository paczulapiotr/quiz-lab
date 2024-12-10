using Microsoft.AspNetCore.SignalR;
using Quiz.Common.Broker.Publisher;

namespace Quiz.Master.Hubs;

internal partial class SyncHub : Hub
{
    private readonly ILogger<SyncHub> _logger;
    private readonly IHubConnection _hubConnection;
    private readonly IPublisher _publisher;

    public SyncHub(ILogger<SyncHub> logger, IHubConnection hubConnection, IPublisher publisher)
    {
        _logger = logger;
        _hubConnection = hubConnection;
        _publisher = publisher;
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
