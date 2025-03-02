
using Microsoft.Extensions.Logging;

namespace Quiz.Common.Hubs;


internal class HubConnection : IHubConnection
{
    private readonly ILogger<HubConnection> logger;
    private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
    private readonly Dictionary<string, string> connections = new();

    public HubConnection(ILogger<HubConnection> logger)
    {
        this.logger = logger;
    }

    public async Task Connected(string connectionId, string targetId)
    {
        await semaphoreSlim.WaitAsync();
        connections.TryAdd(connectionId, targetId);
        semaphoreSlim.Release();
        logger.LogInformation("Client connected - {ConnectionId} for ID - {targetId}", connectionId, targetId);
    }

    public async Task Disconnected(string connectionId)
    {
        await semaphoreSlim.WaitAsync();
        var targetId = connections[connectionId];
        connections.Remove(connectionId);
        semaphoreSlim.Release();
        logger.LogInformation("Client disconnected - {ConnectionId} for ID - {targetId}", connectionId, targetId);
    }

    public string? GetConnectionId(string targetId)
    {
        KeyValuePair<string, string>? pair = connections.FirstOrDefault(x => x.Value == targetId);
        return pair?.Key;
    }

    public async Task WaitForConnection(string targetId, CancellationToken cancellationToken = default)
    {
        while (!connections.Any(x=>x.Value == targetId))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            await Task.Delay(100, cancellationToken);
        }
    }
}