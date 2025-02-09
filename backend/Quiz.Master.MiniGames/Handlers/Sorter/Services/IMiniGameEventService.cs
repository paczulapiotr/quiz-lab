namespace Quiz.Master.MiniGames.Handlers.Sorter;

public interface IMiniGameEventService
{
    public Task Initialize(string gameId, CancellationToken cancellationToken = default);
    public Task SendOnRoundStart(string gameId, CancellationToken cancellationToken = default);
    public Task WaitForRoundStarted(string gameId, CancellationToken cancellationToken = default);
    public Task SendOnRoundEnd(string gameId, CancellationToken cancellationToken = default);
    public Task WaitForRoundSummary(string gameId, CancellationToken cancellationToken = default);

    public Task<SortSelection?> GetSortSelection(string gameId, CancellationToken cancellationToken = default);
}

public record SortSelection(
    Guid PlayerId,
    string ItemId,
    string CategoryId,
    DateTime Timestamp);