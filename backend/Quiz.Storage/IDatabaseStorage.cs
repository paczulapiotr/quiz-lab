using Quiz.Master.Core.Models;

namespace Quiz.Storage;

public interface IDatabaseStorage
{
    Task<Game> FindGameAsync(Guid gameId, CancellationToken cancellationToken = default);
    Task<GameDefinition> FindGameDefinitionAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateGameAsync(Game game, CancellationToken cancellationToken = default);
    Task InsertGameAsync(Game game, CancellationToken cancellationToken = default);
    Task InsertMiniGameAsync<TState>(MiniGameInstance<TState> miniGame, CancellationToken cancellationToken = default)
    where TState : MiniGameStateData, new();
    Task UpdateMiniGameAsync<TState>(MiniGameInstance<TState> miniGame, CancellationToken cancellationToken = default)
    where TState : MiniGameStateData, new();
    Task<MiniGameInstance<TState>> FindMiniGameAsync<TState>(Guid id, CancellationToken cancellationToken = default) 
    where TState : MiniGameStateData, new();
    Task<MiniGameDefinition<TDefinition>> FindMiniGameDefinitionAsync<TDefinition>(Guid id, CancellationToken cancellationToken = default)
    where TDefinition : MiniGameDefinitionData, new();
    Task<IEnumerable<(Player player, Dictionary<Guid, int> scores)>> ListPlayerScores(Guid gameId, CancellationToken cancellationToken = default);
}