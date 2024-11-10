using Microsoft.AspNetCore.SignalR;

namespace Quiz.Slave.Hubs;

public class PingPongHub : Hub
{
    private readonly ILogger<PingPongHub> _logger;

    public PingPongHub(ILogger<PingPongHub> logger)
    {
        _logger = logger;
    }

    public async Task Ping()
    {
        _logger.LogInformation("Ping received from {ConnectionId}", Context.ConnectionId);
        await Clients.Caller.SendAsync("Pong");
    }
}