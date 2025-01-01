
using Microsoft.EntityFrameworkCore;
using Quiz.Common.CQRS;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Master.Persistance;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetPowerPlays;

public record GetPowerPlaysQuery(Guid GameId, string DeviceId) : IQuery<GetPowerPlaysResult>;
public record GetPowerPlaysResult(IEnumerable<Player> Players, IEnumerable<PowerPlay> PowerPlays);
public record Player(string Id, string Name);

public class GetPowerPlaysHandler(IQuizRepository quizRepository) : IQueryHandler<GetPowerPlaysQuery, GetPowerPlaysResult>
{
    public async ValueTask<GetPowerPlaysResult?> HandleAsync(GetPowerPlaysQuery request, CancellationToken cancellationToken = default)
    {
        var miniGame = await quizRepository.Query<MiniGameInstance>()
            .Include(x => x.Game).ThenInclude(x => x.Players)
            .Where(x => x.Game.Id == request.GameId && x.MiniGameDefinition.Type == MiniGameType.AbcdWithCategories)
            .FirstOrDefaultAsync();

        if (miniGame == null)
        {
            throw new InvalidOperationException("Mini game not found");
        }
        var players = miniGame.Game.Players.Where(x => x.DeviceId != request.DeviceId);

        return new GetPowerPlaysResult(
            players.Select(p => new Player(p.DeviceId, p.Name)),
            [PowerPlay.Bombs, PowerPlay.Freeze, PowerPlay.Letters, PowerPlay.Slime]);
    }
}
