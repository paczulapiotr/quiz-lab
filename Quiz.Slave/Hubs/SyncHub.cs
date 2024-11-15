using Microsoft.AspNetCore.SignalR;

namespace Quiz.Slave.Hubs;

public class SyncHub : Hub
{
    private readonly ILogger<SyncHub> _logger;

    public SyncHub(ILogger<SyncHub> logger)
    {
        _logger = logger;
    }

    public async Task Ping(PingHubMessage pingMessage)
    {
        _logger.LogInformation("Ping received from {ConnectionId}, message: {pingMessage}", Context.ConnectionId, pingMessage);
        await Clients.Caller.SendAsync("Pong", new PongHubMessage("11 jabłek", 11));
    }
}

public record PingHubMessage(string? Message, int Amount);
public record PongHubMessage(string? Message, int Amount);