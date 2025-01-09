
using Quiz.Master.Core.Models;

namespace Quiz.Master.Game.MiniGames;

public interface IMiniGameRepository
{
    Task<MiniGameInstance<TState>> FindMiniGameAsync<TState>(Guid miniGameId, CancellationToken cancellationToken = default)
    where TState : MiniGameStateData, new();

    Task<MiniGameDefinition<TDefinition>> FindMiniGameDefinitionAsync<TDefinition>(Guid definitionId, CancellationToken cancellationToken = default)
    where TDefinition : MiniGameDefinitionData, new();

    Task InsertMiniGameAsync<TState>(MiniGameInstance<TState> miniGame, CancellationToken cancellationToken = default)
    where TState : MiniGameStateData, new();

    Task UpdateMiniGameStateAsync<TState>(Guid miniGameId, TState stateData, CancellationToken cancellationToken = default)
    where TState : MiniGameStateData, new();

    Task UpsertPlayerScoreAsync(Guid miniGameId, Guid playerId, int score, CancellationToken cancellationToken = default);
}