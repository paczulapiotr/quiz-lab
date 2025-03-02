namespace Quiz.Master.MiniGames.Handlers;

public abstract class CommonSelection<TSelection, TSelectionState>
where TSelection : class
where TSelectionState : class
{
    public record Options(int SelectionTimeMs, IEnumerable<string> PlayerIds);

    public async Task<TSelectionState?> Select(
        string gameId,
        TSelectionState? initialState = null,
        Options? options = null,
        CancellationToken cancellationToken = default)

    {
        var timeToken = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,
            options is null ? default : new CancellationTokenSource(options.SelectionTimeMs).Token)
            .Token;
        var selections = initialState;
        var awaitingPlayerIds = options?.PlayerIds?.ToList();

        while (!timeToken.IsCancellationRequested && (awaitingPlayerIds is null || awaitingPlayerIds.Any()))
        {
            var (playerId, selection) = await SelectOne(gameId, timeToken);
            if (playerId is not null)
            {
                awaitingPlayerIds?.Remove(playerId);
            }

            if (selection is null || timeToken.IsCancellationRequested)
            {
                break;
            }

            selections = UpdateSelectionState(selection, selections);
        }

        selections = FinalizeState(selections);

        return selections;
    }

    protected abstract Task<(string? playerId, TSelection?)> SelectOne(string gameId, CancellationToken cancellationToken);

    protected abstract TSelectionState? UpdateSelectionState(TSelection selection, TSelectionState? state);

    protected virtual TSelectionState? FinalizeState(TSelectionState? state) => state;
}