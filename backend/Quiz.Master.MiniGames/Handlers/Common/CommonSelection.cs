namespace Quiz.Master.MiniGames.Handlers;

public abstract class CommonSelection<TSelection, TSelectionState>
where TSelection : class
where TSelectionState : class, new()
{
    public async Task<TSelectionState> Select(
        string gameId,
        IEnumerable<string> PlayerIds,
        TSelectionState initialState,
        int selectionTimeMs,
        CancellationToken cancellationToken)

    {
        var timeToken = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,
            new CancellationTokenSource(selectionTimeMs).Token)
            .Token;
        var selections = initialState;
        var awaitingPlayerIds = PlayerIds.ToList();

        while (!timeToken.IsCancellationRequested && awaitingPlayerIds.Any())
        {
            var (playerId, selection) = await SelectOne(gameId, timeToken);
            if(playerId is not null) {
                awaitingPlayerIds.Remove(playerId);
            }

            if (selection is null || timeToken.IsCancellationRequested)
            {
                break;
            }

            selections = UpdateSelectionState(selection, selections);
        }

        return selections;
    }

    protected abstract Task<(string? playerId, TSelection?)> SelectOne(string gameId, CancellationToken cancellationToken);

    protected abstract TSelectionState UpdateSelectionState(TSelection selection, TSelectionState state);
}