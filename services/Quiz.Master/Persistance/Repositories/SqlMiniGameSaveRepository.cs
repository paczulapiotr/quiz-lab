using System.Text.Json;
using Quiz.Master.Persistance.Models;
using Quiz.Master.Persistance.Models.MiniGames;
using Quiz.Master.Persistance.Repositories.Abstract;

namespace Quiz.Master.Persistance.Repositories;

public class SqlMiniGameSaveRepository(QuizDbContext quizDbContext) : IMiniGameSaveRepository
{
    public async Task SaveMiniGame<TData, TConfig, TState>(MiniGameInstance miniGame, TData stateData, CancellationToken cancellationToken = default)
    where TData : MiniGameDataBase<TConfig, TState>
    where TConfig : class, new()
    where TState : class, new()
    {
        var jsonData = JsonSerializer.Serialize(stateData);
        miniGame.RoundsJsonData = jsonData;
        quizDbContext.Update(miniGame);
        await quizDbContext.SaveChangesAsync(cancellationToken);
    }
}
