
using Quiz.Common.CQRS;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Storage;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetPowerPlays;

public record GetPowerPlaysQuery(Guid GameId, string DeviceId) : IQuery<GetPowerPlaysResult>;
public record GetPowerPlaysResult(IEnumerable<Player> Players, IEnumerable<PowerPlay> PowerPlays);
public record Player(string Id, string Name);

public class GetPowerPlaysHandler(IDatabaseStorage storage) : IQueryHandler<GetPowerPlaysQuery, GetPowerPlaysResult>
{
    public async ValueTask<GetPowerPlaysResult?> HandleAsync(GetPowerPlaysQuery request, CancellationToken cancellationToken = default)
    {
        var game = await storage.FindGameAsync(request.GameId, cancellationToken);
        var players = game.Players.Where(x => x.DeviceId != request.DeviceId);

        return new GetPowerPlaysResult(
            players.Select(p => new Player(p.DeviceId, p.Name)),
            [PowerPlay.Bombs, PowerPlay.Freeze, PowerPlay.Letters, PowerPlay.Slime]);
    }
}
