using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Quiz.Master.Persistance.Models;
using Quiz.Master.Persistance.Repositories.Abstract;

namespace Quiz.Master.Persistance.Repositories;

public class SqlMiniGameSaveRepository(IDbContextFactory<QuizDbContext> quizDbContextFactory) : IMiniGameSaveRepository
{
    public async Task SaveMiniGame<TState>(MiniGameInstance miniGame, TState stateData, CancellationToken cancellationToken = default)
    where TState : class, new()
    {
        await using var quizDbContext = quizDbContextFactory.CreateDbContext();
        var jsonData = JsonSerializer.Serialize(stateData);
        miniGame.StateJsonData = jsonData;
        quizDbContext.Update(miniGame);
        await quizDbContext.SaveChangesAsync(cancellationToken);
    }
}
