using Microsoft.AspNetCore.SignalR;
using Quiz.Slave.Hubs.Models;

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

    public Task SelectAnswer(SelectAnswer answer)
    {
        _logger.LogInformation("SelectAnswer received from {ConnectionId}, answer: {answer}", Context.ConnectionId, answer);
        return Task.CompletedTask;
    }
}

public record PingHubMessage(string? Message, int Amount);
public record PongHubMessage(string? Message, int Amount);