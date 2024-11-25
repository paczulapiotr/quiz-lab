using Quiz.Slave.Hubs.Models;

namespace Quiz.Slave.Hubs;

internal interface ISyncHubClient
{
    Task SelectAnswer(SelectAnswer payload, CancellationToken cancellationToken = default);
    Task GameCreated(GameCreatedSyncMessage payload, CancellationToken cancellationToken = default);
}