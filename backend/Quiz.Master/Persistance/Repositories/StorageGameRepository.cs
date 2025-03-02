
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

    public async Task<(Room? room, string? errorCode)> RegisterHostAsync(string hostId, string roomCode, CancellationToken cancellationToken = default)
    {
        var existingRoom = await storage.FindRoomByCodeAsync(roomCode, cancellationToken);

        if (existingRoom?.IsOpen == false)
        {
            return (null, "RoomAlreadyExists");
        }

        var room = new Room
        {
            Code = roomCode,
            HostDeviceId = hostId,
            PlayerDeviceIds = [],
            CreatedAt = DateTime.UtcNow,
            IsOpen = true,
        };

        await storage.InsertRoomAsync(room, cancellationToken);

        return (room, null);
    }

    public async Task<(Room? room, string? errorCode)> RegisterPlayerAsync(string deviceId, string roomCode, CancellationToken cancellationToken = default)
    {
        var room = await storage.FindRoomByCodeAsync(roomCode, cancellationToken);
        if (room is null) return (null, "RoomNotFound");

        if (room.IsOpen == false)
        {
            return (null, "RoomClosed");
        }
        
        room.PlayerDeviceIds ??= new HashSet<string>();
        room.PlayerDeviceIds.Add(deviceId);

        await storage.UpdateRoomAsync(room, cancellationToken);

        return (room, null);
    }

    public async Task CloseRoomAsync(string roomCode, CancellationToken cancellationToken = default)
    {
        var room = await storage.FindRoomByCodeAsync(roomCode, cancellationToken);
        if (room is null) return;

        room.IsOpen = false;

        await storage.UpdateRoomAsync(room, cancellationToken);
    }

    public async Task UpdateAsync(Core.Models.Game game, CancellationToken cancellationToken = default)
    {
        await storage.UpdateGameAsync(game, cancellationToken);
    }
}
