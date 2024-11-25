using Microsoft.AspNetCore.SignalR;
using Quiz.Slave.Hubs.Models;

namespace Quiz.Slave.Hubs;

public interface ISyncHubWait
{
    Task WaitForClient(CancellationToken cancellationToken = default);
}

public partial class SyncHub : Hub
{
    private readonly ILogger<SyncHub> _logger;
    private readonly IHubConnection _hubConnection;

    public SyncHub(ILogger<SyncHub> logger, IHubConnection hubConnection)
    {
        _logger = logger;
        _hubConnection = hubConnection;
    }

    public Task SelectAnswer(SelectAnswer answer)
    {
        _logger.LogInformation("SelectAnswer received from {ConnectionId}, answer: {answer}", Context.ConnectionId, answer);
        return Task.CompletedTask;
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
