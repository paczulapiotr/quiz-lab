
using Microsoft.EntityFrameworkCore;
using Quiz.Common.CQRS;
using Quiz.Master.Extensions;
using Quiz.Master.Persistance;

namespace Quiz.Master.Features.Game.GetGame;

public record GetGameResult(string GameId, uint GameSize, IEnumerable<string> PlayerNames, string[] Rounds, bool IsStarted = false, bool IsFinished = false, string? YourPlayerName = null, string? YourDeviceId = null);
public record GetGameQuery(Guid GameId) : IQuery<GetGameResult>;

public class GetGameHandler(IQuizRepository quizRepository, IHttpContextAccessor httpContextAccessor) : IQueryHandler<GetGameQuery, GetGameResult>
{

    public async ValueTask<GetGameResult?> HandleAsync(GetGameQuery request, CancellationToken cancellationToken = default)
    {
        var activeGame = await quizRepository.Query<Persistance.Models.Game>()
            .Include(x => x.Players)
            .Where(x => x.Id == request.GameId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        var player = activeGame?.Players.FirstOrDefault(x => x.DeviceId == httpContextAccessor.GetDeviceId());

        return activeGame is null
            ? new GetGameResult(string.Empty, 0, [], [])
            : new GetGameResult(
                activeGame.Id.ToString(),
                activeGame.GameSize,
                activeGame.Players.OrderByDescending(x => x.CreatedAt).Select(x => x.Name),
                activeGame.Rounds.Select(x => x.ToString()).ToArray(),
                activeGame.IsStarted,
                activeGame.IsFinished,
                player?.Name,
                player?.DeviceId
                );
    }
}
