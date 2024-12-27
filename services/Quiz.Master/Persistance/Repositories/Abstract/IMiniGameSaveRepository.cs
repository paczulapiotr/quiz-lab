
namespace Quiz.Master.Persistance.Repositories.Abstract;

public interface IMiniGameSaveRepository
{
    Task SaveMiniGameState<TState>(Guid miniGameId, TState stateData, CancellationToken cancellationToken = default)
    where TState : class, new();

    Task AddPlayerScore(Guid miniGameId, Guid playerId, int score, CancellationToken cancellationToken = default);
}