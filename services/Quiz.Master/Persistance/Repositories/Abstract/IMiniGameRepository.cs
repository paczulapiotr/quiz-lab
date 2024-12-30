
namespace Quiz.Master.Persistance.Repositories.Abstract;

public interface IMiniGameRepository
{
    Task UpdateStateAsync<TState>(Guid miniGameId, TState stateData, CancellationToken cancellationToken = default)
    where TState : class, new();

    Task UpsertPlayerScoreAsync(Guid miniGameId, string playerId, int score, CancellationToken cancellationToken = default);
}