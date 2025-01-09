
using Quiz.Common.CQRS;
using Quiz.Master.Extensions;
using Quiz.Storage;

namespace Quiz.Master.Features.Game.GetGame;

public record GetGameResult(string GameId, uint GameSize, IEnumerable<string> PlayerNames, string[] Rounds, bool IsStarted = false, bool IsFinished = false, string? YourPlayerName = null, string? YourDeviceId = null);
public record GetGameQuery(Guid GameId) : IQuery<GetGameResult>;

public class GetGameHandler(IDatabaseStorage storage, IHttpContextAccessor httpContextAccessor) : IQueryHandler<GetGameQuery, GetGameResult>
{

    public async ValueTask<GetGameResult?> HandleAsync(GetGameQuery request, CancellationToken cancellationToken = default)
    {
        var game = await storage.FindGameAsync(request.GameId, cancellationToken);
        
        if(game is null || game.IsFinished) {
            return new GetGameResult(string.Empty, 0, [], []);
        }

        var player = game.Players.FirstOrDefault(x => x.DeviceId == httpContextAccessor.GetDeviceId());

        return game is null
            ? new GetGameResult(string.Empty, 0, [], [])
            : new GetGameResult(
                game.Id.ToString(),
                game.GameSize,
                game.Players.OrderByDescending(x => x.CreatedAt).Select(x => x.Name),
                game.MiniGames.Select(x => x.ToString()).ToArray(),
                game.IsStarted,
                game.IsFinished,
                player?.Name,
                player?.DeviceId
                );
    }
}
