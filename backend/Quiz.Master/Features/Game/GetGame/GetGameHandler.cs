
using Quiz.Common.CQRS;
using Quiz.Common.Messages.Game;
using Quiz.Master.Extensions;
using Quiz.Storage;

namespace Quiz.Master.Features.Game.GetGame;

public record GetGameResult(string GameId, uint GameSize, GameStatus? GameStatus, IEnumerable<GamePlayer> Players, bool IsStarted = false, bool IsFinished = false, string? YourPlayerName = null, string? YourDeviceId = null);
public record GamePlayer(string Id, string DeviceId, string Name);
public record GetGameQuery(Guid GameId) : IQuery<GetGameResult>;

public class GetGameHandler(IDatabaseStorage storage, IHttpContextAccessor httpContextAccessor) : IQueryHandler<GetGameQuery, GetGameResult>
{

    public async ValueTask<GetGameResult?> HandleAsync(GetGameQuery request, CancellationToken cancellationToken = default)
    {
        var game = await storage.FindGameAsync(request.GameId, cancellationToken);
        
        if(game is null || game.IsFinished) {
            return new GetGameResult(string.Empty, 0, null, []);
        }

        var deviceId = httpContextAccessor.GetUniqueId();
        var player = game.Players.FirstOrDefault(x => x.DeviceId == deviceId);

        return game is null
            ? new GetGameResult(string.Empty, 0, null, [])
            : new GetGameResult(
                game.Id.ToString(),
                game.GameSize,
                (GameStatus)game.Status,
                game.Players.OrderBy(x => x.CreatedAt).Select(x => new GamePlayer(x.Id.ToString(), x.DeviceId, x.Name)),
                game.IsStarted,
                game.IsFinished,
                player?.Name,
                player?.DeviceId
                );
    }
}
