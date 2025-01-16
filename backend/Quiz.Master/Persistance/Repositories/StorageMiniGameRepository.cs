using Quiz.Master.Core.Models;
using Quiz.Master.Game.MiniGames;
using Quiz.Storage;

namespace Quiz.Master.Persistance.Repositories;

public class StorageMiniGameRepository(IDatabaseStorage databaseStorage) : IMiniGameRepository
{
    public async Task<MiniGameInstance> FindMiniGameAsync(Guid miniGameId, CancellationToken cancellationToken = default)
    {
        return await databaseStorage.FindMiniGameAsync(miniGameId, cancellationToken);
    }

    public async Task<MiniGameDefinition> FindMiniGameDefinitionAsync(Guid definitionId, CancellationToken cancellationToken = default)
    {
        return await databaseStorage.FindMiniGameDefinitionAsync(definitionId, cancellationToken);
    }

    public async Task UpdateMiniGameStateAsync(Guid miniGameId, MiniGameStateData stateData, CancellationToken cancellationToken = default)
    {
        var miniGame = await databaseStorage.FindMiniGameAsync(miniGameId, cancellationToken);
        miniGame.State = stateData;

        await databaseStorage.UpdateMiniGameAsync(miniGame, cancellationToken);
    }

    public async Task InsertMiniGameAsync(MiniGameInstance miniGame, CancellationToken cancellationToken = default)
    {
        await databaseStorage.InsertMiniGameAsync(miniGame, cancellationToken);
    }

    public async Task UpsertPlayerScoreAsync(Guid miniGameId, Guid playerId, int score, CancellationToken cancellationToken = default)
    {
        var miniGame = await databaseStorage.FindMiniGameAsync(miniGameId, cancellationToken);

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
                Score = score
            };
            miniGame.PlayerScores.Add(playerScore);
        }
        else
        {
            playerScore.Score += score;
        }

        await databaseStorage.UpdateMiniGameAsync(miniGame, cancellationToken);
    }
}
