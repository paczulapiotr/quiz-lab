using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Quiz.Master.Core.Models;
using Quiz.Master.Persistance.Repositories.Abstract;

namespace Quiz.Master.Persistance.Repositories;

public class SqlMiniGameRepository(IDbContextFactory<QuizDbContext> quizDbContextFactory) : IMiniGameRepository
{
    public async Task UpdateStateAsync<TState>(Guid miniGameId, TState stateData, CancellationToken cancellationToken = default)
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


    public async Task UpsertPlayerScoreAsync(Guid miniGameId, string playerId, int score, CancellationToken cancellationToken = default)
    {
        await using var quizDbContext = quizDbContextFactory.CreateDbContext();
        var miniGame = await quizDbContext.MiniGameInstances
            .Include(x => x.PlayerScores)
            .Include(x => x.Game).ThenInclude(x => x.Players)
            .FirstOrDefaultAsync(x => x.Id == miniGameId);

        var player = miniGame?.Game.Players.FirstOrDefault(x => x.DeviceId == playerId);

        if (miniGame == null)
        {
            throw new InvalidOperationException("Mini game not found");
        }

        var playerScore = miniGame.PlayerScores.FirstOrDefault(x => x.PlayerId == player?.Id);

        if (playerScore == null)
        {
            playerScore = new MiniGameInstanceScore
            {
                Player = player,
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
