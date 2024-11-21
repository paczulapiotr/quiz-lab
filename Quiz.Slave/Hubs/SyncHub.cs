using Microsoft.AspNetCore.SignalR;
using Quiz.Slave.Hubs.Models;

namespace Quiz.Slave.Hubs;

public partial class SyncHub : Hub
{
    private readonly ILogger<SyncHub> _logger;

    public SyncHub(ILogger<SyncHub> logger)
    {
        _logger = logger;
    }

    public Task SelectAnswer(SelectAnswer answer)
    {
        _logger.LogInformation("SelectAnswer received from {ConnectionId}, answer: {answer}", Context.ConnectionId, answer);
        return Task.CompletedTask;
    }
}
