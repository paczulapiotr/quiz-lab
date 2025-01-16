using Quiz.Master.Core.Models;

namespace Quiz.Storage;

public interface IDatabaseStorage
{
    Task<Game> FindGameAsync(Guid gameId, CancellationToken cancellationToken = default);
    Task UpdateGameAsync(Game game, CancellationToken cancellationToken = default);
    Task InsertGameAsync(Game game, CancellationToken cancellationToken = default);
    Task InsertGameDefinitionAsync(GameDefinition gameDefinition, CancellationToken cancellationToken = default);
    Task<GameDefinition> FindGameDefinitionAsync(Guid id, CancellationToken cancellationToken = default);
    Task<MiniGameInstance> FindMiniGameAsync(Guid id, CancellationToken cancellationToken = default);
    Task InsertMiniGameAsync(MiniGameInstance miniGame, CancellationToken cancellationToken = default);
    Task UpdateMiniGameAsync(MiniGameInstance miniGame, CancellationToken cancellationToken = default);
    Task InsertManyMiniGameDefinitionAsync(IEnumerable<MiniGameDefinition> gameDefinitions, CancellationToken cancellationToken = default);
    Task<MiniGameDefinition> FindMiniGameDefinitionAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<(Player player, Dictionary<Guid, int> scores)>> ListPlayerScores(Guid gameId, CancellationToken cancellationToken = default);
}