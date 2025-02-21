using Quiz.Master.Core.Models;

namespace Quiz.Storage;

public interface IDatabaseStorage
{
    Task<Room?> FindRoomByCodeAsync(string roomCode, CancellationToken cancellationToken = default);
    Task UpdateRoomAsync(Room room, CancellationToken cancellationToken = default);
    Task InsertRoomAsync(Room room, CancellationToken cancellationToken = default);
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
    Task<(TState?, TDefinition?)> FindCurrentMiniGameStateAndDefinitionAsync<TState, TDefinition>(Guid gameId, CancellationToken cancellationToken = default)
    where TState : MiniGameStateData, new()
    where TDefinition : MiniGameDefinitionData, new();
}