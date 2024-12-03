
using Microsoft.EntityFrameworkCore;
using Quiz.Common.CQRS;
using Quiz.Master.Persistance;
using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Features.Game.GetGame;

public record GetMiniGameResult(string? MiniGameId = null, MiniGameType? MiniGameType = null);
public record GetMiniGameQuery(Guid GameId) : IQuery<GetMiniGameResult>;

public class GetMiniGameHandler(IQuizRepository quizRepository) : IQueryHandler<GetMiniGameQuery, GetMiniGameResult>
{

    public async ValueTask<GetMiniGameResult?> HandleAsync(GetMiniGameQuery request, CancellationToken cancellationToken = default)
    {
        var activeGame = await quizRepository.Query<Persistance.Models.Game>()
            .Include(x => x.Players)
            .Include(x => x.MiniGames)
            .Where(x => x.Id == request.GameId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        var currentMiniGame = activeGame?.MiniGames.FirstOrDefault(x => x.Type == activeGame.CurrentMiniGame);

        return currentMiniGame is null ? new GetMiniGameResult() : new GetMiniGameResult(currentMiniGame.Id.ToString(), currentMiniGame.Type);
    }
}
