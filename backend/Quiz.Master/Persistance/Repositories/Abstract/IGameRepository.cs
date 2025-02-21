using Quiz.Master.Core.Models;

namespace Quiz.Master.Persistance.Repositories.Abstract;

public interface IGameRepository
{
    Task<Room> RegisterHostAsync(string hostId, string roomCode, CancellationToken cancellationToken = default);
    Task<Room?> RegisterPlayerAsync(string deviceId, string roomCode, CancellationToken cancellationToken = default);
    Task<Room?> FindRoomByCodeAsync(string roomCode, CancellationToken cancellationToken = default);
    Task UpdateAsync(Core.Models.Game game, CancellationToken cancellationToken = default);
    Task<Core.Models.Game> FindAsync(Guid gameId, CancellationToken cancellationToken = default);
    Task<GameDefinition> FindGameDefinitionAsync(Guid gameDefinitionId, CancellationToken cancellationToken = default);
}