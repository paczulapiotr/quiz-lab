
using Quiz.Master.Persistance.Models;
using Quiz.Master.Persistance.Models.MiniGames;

namespace Quiz.Master.Persistance.Repositories.Abstract;

public interface IMiniGameSaveRepository
{
    Task SaveMiniGame<TData, TConfig, TState>(MiniGameInstance miniGame, TData stateData, CancellationToken cancellationToken = default)
    where TData : MiniGameDataBase<TConfig, TState>
    where TConfig : class, new()
    where TState : class, new();
}