using Quiz.Slave.Hubs.Models;

namespace Quiz.Slave.Hubs;

internal interface ISyncHubClient
{
    Task SelectAnswer(SelectAnswer payload, CancellationToken cancellationToken = default);
    Task GameCreated(GameCreatedSyncMessage payload, CancellationToken cancellationToken = default);
    Task PlayerJoined(PlayerJoinedSyncMessage payload, CancellationToken cancellationToken = default);
    Task GameStarting(string gameId, CancellationToken cancellationToken = default);
    Task GameStarted(string gameId, CancellationToken cancellationToken = default);

    Task GameEnd(string gameId, CancellationToken cancellationToken = default);
    Task RoundEnd(string gameId, CancellationToken cancellationToken = default);
    Task RoundStart(string gameId, CancellationToken cancellationToken = default);
    Task RulesExplain(string gameId, CancellationToken cancellationToken = default);
    Task RulesExplained(string gameId, CancellationToken cancellationToken = default);
}