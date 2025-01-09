using Quiz.Master.Core.Models;
using Quiz.Master.Game.MiniGames;
using Quiz.Storage;

namespace Quiz.Master.Persistance.Repositories;

public class StorageMiniGameRepository(IDatabaseStorage databaseStorage) : IMiniGameRepository
{
    public async Task<MiniGameInstance<TState>> FindMiniGameAsync<TState>(Guid miniGameId, CancellationToken cancellationToken = default) where TState : MiniGameStateData, new()
    {
        return await databaseStorage.FindMiniGameAsync< TState>(miniGameId, cancellationToken);
    }

    public async Task<MiniGameDefinition<TDefinition>> FindMiniGameDefinitionAsync<TDefinition>(Guid definitionId, CancellationToken cancellationToken = default) where TDefinition : MiniGameDefinitionData, new()
    {
        return await databaseStorage.FindMiniGameDefinitionAsync<TDefinition>(definitionId, cancellationToken);
    }

    public async Task UpdateMiniGameStateAsync<TState>(Guid miniGameId, TState stateData, CancellationToken cancellationToken = default)
    where TState : MiniGameStateData, new()
    {
        var miniGame = await databaseStorage.FindMiniGameAsync<TState>(miniGameId, cancellationToken);
        miniGame.State = stateData;

        await databaseStorage.UpdateMiniGameAsync(miniGame, cancellationToken);
    }

    public async Task InsertMiniGameAsync<TState>(MiniGameInstance<TState> miniGame, CancellationToken cancellationToken = default)
    where TState : MiniGameStateData, new()
    {
        await databaseStorage.InsertMiniGameAsync(miniGame, cancellationToken);
    }

    public async Task UpsertPlayerScoreAsync(Guid miniGameId, Guid playerId, int score, CancellationToken cancellationToken = default)
    {
        var miniGame = await databaseStorage.FindMiniGameAsync<MiniGameStateData>(miniGameId, cancellationToken);

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
