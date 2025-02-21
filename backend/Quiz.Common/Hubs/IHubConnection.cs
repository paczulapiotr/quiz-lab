namespace Quiz.Common.Hubs;

public interface IHubConnection
{
    string? GetConnectionId(string targetId);
    Task WaitForConnection(string targetId, CancellationToken cancellationToken = default);
    Task Connected(string connectionId, string targetId);
    Task Disconnected(string connectionId);
}