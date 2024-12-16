
using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Persistance.Repositories.Abstract;

public interface IMiniGameSaveRepository
{
    Task SaveMiniGame<TState>(MiniGameInstance miniGame, TState stateData, CancellationToken cancellationToken = default)
    where TState : class, new();
}