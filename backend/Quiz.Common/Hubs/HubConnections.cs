
using Microsoft.Extensions.Logging;

namespace Quiz.Common.Hubs;


internal class HubConnection : IHubConnection
{
    private readonly ILogger<HubConnection> logger;
    private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
    private readonly List<string> connections = new();

    public HubConnection(ILogger<HubConnection> logger)
    {
        this.logger = logger;
    }

    public async Task Connected(string connectionId)
    {
        await semaphoreSlim.WaitAsync();
        connections.Add(connectionId);
        semaphoreSlim.Release();
        logger.LogInformation("Client connected: {ConnectionId}", connectionId);
    }

    public async Task Disconnected(string connectionId)
    {
        await semaphoreSlim.WaitAsync();
        connections.Remove(connectionId);
        semaphoreSlim.Release();
        logger.LogInformation("Client disconnected: {ConnectionId}", connectionId);
    }


    public async Task WaitForConnection(CancellationToken cancellationToken = default)
    {
        while (!connections.Any())
        {
            if (cancellationToken.IsCancellationRequested)
            {
                logger.LogInformation("WaitForConnection cancelled");
                return;
            }
            await Task.Delay(100, cancellationToken);
        }
    }
}