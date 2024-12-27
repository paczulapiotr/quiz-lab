using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Quiz.Master.Persistance.Models;
using Quiz.Master.Persistance.Repositories.Abstract;

namespace Quiz.Master.Persistance.Repositories;

public class SqlMiniGameSaveRepository(IDbContextFactory<QuizDbContext> quizDbContextFactory) : IMiniGameSaveRepository
{
    public async Task SaveMiniGameState<TState>(Guid miniGameId, TState stateData, CancellationToken cancellationToken = default)
    where TState : class, new()
    {
        await using var quizDbContext = quizDbContextFactory.CreateDbContext();
        var miniGame = await quizDbContext.MiniGameInstances.FirstOrDefaultAsync(x => x.Id == miniGameId);
        if (miniGame == null)
        {
            throw new InvalidOperationException("Mini game not found");
        }

        var jsonData = JsonSerializer.Serialize(stateData);
        miniGame.StateJsonData = jsonData;
        quizDbContext.Update(miniGame);

        await quizDbContext.SaveChangesAsync(cancellationToken);
    }


    public async Task AddPlayerScore(Guid miniGameId, Guid playerId, int score, CancellationToken cancellationToken = default)
    {
        await using var quizDbContext = quizDbContextFactory.CreateDbContext();
        var miniGame = await quizDbContext.MiniGameInstances
            .Include(x => x.PlayerScores)
            .FirstOrDefaultAsync(x => x.Id == miniGameId);

        if (miniGame == null)
        {
            throw new InvalidOperationException("Mini game not found");
        }

        var playerScore = miniGame.PlayerScores.FirstOrDefault(x => x.PlayerId == playerId);
        if (playerScore == null)
        {
            playerScore = new MiniGameInstanceScore
            {
                PlayerId = playerId,
                MiniGameInstanceId = miniGameId,
                Score = score
            };
            quizDbContext.MiniGameInstanceScores.Add(playerScore);
        }
        else
        {
            playerScore.Score += score;
            quizDbContext.Update(miniGame);
        }

        await quizDbContext.SaveChangesAsync(cancellationToken);
    }
}
