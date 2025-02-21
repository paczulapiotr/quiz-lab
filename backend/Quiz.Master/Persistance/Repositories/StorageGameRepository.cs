
using Quiz.Master.Core.Models;
using Quiz.Master.Persistance.Repositories.Abstract;
using Quiz.Storage;

namespace Quiz.Master.Persistance.Repositories;

public class StorageGameRepository(IDatabaseStorage storage) : IGameRepository
{
    public async Task<Core.Models.Game> FindAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        return await storage.FindGameAsync(gameId, cancellationToken);
    }

    public async Task<GameDefinition> FindGameDefinitionAsync(Guid gameDefinitionId, CancellationToken cancellationToken = default)
    {
        return await storage.FindGameDefinitionAsync(gameDefinitionId, cancellationToken);
    }

    public async Task<Room?> FindRoomByCodeAsync(string roomCode, CancellationToken cancellationToken = default)
    {
        return await storage.FindRoomByCodeAsync(roomCode, cancellationToken);
    }

    public async Task<Room> RegisterHostAsync(string hostId, string roomCode, CancellationToken cancellationToken = default)
    {
        var room = new Room
        {
            Code = roomCode,
            HostDeviceId = hostId,
            PlayerDeviceIds = []
        };

        await storage.InsertRoomAsync(room, cancellationToken);

        return room;
    }

    public async Task<Room?> RegisterPlayerAsync(string deviceId, string roomCode, CancellationToken cancellationToken = default)
    {
        var room = await storage.FindRoomByCodeAsync(roomCode, cancellationToken);
        if (room is null) return null;

        room.PlayerDeviceIds ??= new HashSet<string>();
        room.PlayerDeviceIds.Add(deviceId);

        await storage.UpdateRoomAsync(room, cancellationToken);

        return room;
    }

    public async Task UpdateAsync(Core.Models.Game game, CancellationToken cancellationToken = default)
    {
        await storage.UpdateGameAsync(game, cancellationToken);
    }
}
