namespace Quiz.Slave.Hubs;

public interface IHubConnection
{
    Task WaitForConnection(CancellationToken cancellationToken = default);
    Task Connected(string connectionId);
    Task Disconnected(string connectionId);
}