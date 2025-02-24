using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Quiz.Common.Hubs;
using Quiz.Common.Messages.Game;
using Quiz.Master.Hubs.Models;
using Quiz.Master.Persistance.Repositories.Abstract;

namespace Quiz.Master.Hubs;

internal class SyncHubClient : SyncHubClientBase<SyncHub>, ISyncHubClient
{
    private readonly IGameRepository repository;
    private readonly IMemoryCache cache;
    private readonly TimeSpan cacheExpiration = TimeSpan.FromMinutes(5);
    public SyncHubClient(IHubContext<SyncHub> ctx, IHubConnection hubConnection, IGameRepository repository, IMemoryCache cache) : base(ctx, hubConnection)
    {
        this.repository = repository;
        this.cache = cache;
    }

    private async Task<IEnumerable<string>> GetTargetIds(string gameId, bool fresh = false)
    {
        if (!fresh
        && cache.TryGetValue(gameId, out List<string>? targetIds)
        && targetIds != null && targetIds.Any())
        {
            return targetIds;
        }
        if (!Guid.TryParse(gameId, out var guid))
        {
            return Array.Empty<string>();
        }

        var game = await repository.FindAsync(guid);

        if (game is null) return [];

        var room = await repository.FindRoomByCodeAsync(game.RoomCode);

        if (room is null) return [];

        var ids = room.PlayerDeviceIds.Append(room.HostDeviceId);
        
        cache.Set(gameId, ids, cacheExpiration);

        return ids;
    }


    public async Task GameStatusUpdated(GameStatusUpdateSyncMessage payload, CancellationToken cancellationToken = default)
    {
        var refresh = payload.Status == GameStatus.GameStarting || payload.Status == GameStatus.GameJoined;
        var targetIds = await GetTargetIds(payload.GameId, refresh);

        await SendAsync(
                SyncDefinitions.SendGameStatusUpdate,
                payload,
                targetIds,
                cancellationToken);
    }

    public async Task MiniGameNotification(MiniGameNotificationSyncMessage payload, CancellationToken cancellationToken = default)
    {
        var targetIds = await GetTargetIds(payload.GameId);
        await SendAsync(
                   SyncDefinitions.SendMiniGameNotification,
                   payload,
                   targetIds,
                   cancellationToken);
    }

    public async Task MiniGameUpdated(MiniGameUpdateSyncMessage payload, CancellationToken cancellationToken = default)
    {
        var targetIds = await GetTargetIds(payload.GameId);
        await SendAsync(
                  SyncDefinitions.SendMiniGameUpdate,
                  payload,
                  targetIds,
                  cancellationToken);
    }
}