
using Quiz.Master.Core.Models;

namespace Quiz.Master.Game.MiniGames;

public interface IMiniGameRepository
{
    Task<MiniGameInstance> FindMiniGameAsync(Guid miniGameId, CancellationToken cancellationToken = default);
    Task<MiniGameDefinition> FindMiniGameDefinitionAsync(Guid definitionId, CancellationToken cancellationToken = default);
    Task InsertMiniGameAsync(MiniGameInstance miniGame, CancellationToken cancellationToken = default);
    Task UpdateMiniGameStatusAsync(Guid miniGameId, string miniGameStatus, CancellationToken cancellationToken = default);
    Task UpdateMiniGameStateAsync(Guid miniGameId, MiniGameStateData stateData, CancellationToken cancellationToken = default);
    Task UpsertPlayerScoreAsync(Guid miniGameId, Guid playerId, int score, CancellationToken cancellationToken = default);
}