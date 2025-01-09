
using Quiz.Common.CQRS;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Storage;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetAppliedPowerPlay;

public record GetAppliedPowerPlayQuery(Guid GameId, string DeviceId) : IQuery<GetAppliedPowerPlayResult>;
public record GetAppliedPowerPlayResult(IEnumerable<PlayersAppliedPower> players);
public record PlayersAppliedPower(Guid PlayerId, string PlayerName, List<AppliedPowerPlay> PowerPlays);
public record AppliedPowerPlay(Guid PlayerId, string PlayerName, PowerPlay PowerPlay);

public class GetAppliedPowerPlayHandler(IDatabaseStorage storage) : IQueryHandler<GetAppliedPowerPlayQuery, GetAppliedPowerPlayResult>
{

    public async ValueTask<GetAppliedPowerPlayResult?> HandleAsync(GetAppliedPowerPlayQuery request, CancellationToken cancellationToken = default)
    {
        var game = await storage.FindGameAsync(request.GameId, cancellationToken);
        var miniGame = await storage.FindMiniGameAsync<AbcdWithCategoriesState>(game.CurrentMiniGameId!.Value, cancellationToken);
        var miniGameDefinition = await storage.FindMiniGameDefinitionAsync<AbcdWithCategoriesDefinition>(miniGame.MiniGameDefinitionId, cancellationToken);
        var state = miniGame.State;
        var players = game.Players;

        var playersToMap = string.IsNullOrWhiteSpace(request.DeviceId)
            ? players
            : players.Where(x => x.DeviceId == request.DeviceId).ToList();

        var powerPlays = state.Rounds.LastOrDefault()?.PowerPlays;

        var playersAppliedPower = new List<PlayersAppliedPower>();
        foreach (var player in playersToMap)
        {
            var powerPlaysForPlayer = powerPlays?.FirstOrDefault(x => x.Key == player.Id).Value;
            var playerAppliedPowerPlay = new PlayersAppliedPower(
                player.Id,
                player.Name,
                powerPlaysForPlayer?.Select(x
                    => new AppliedPowerPlay(
                        x.FromPlayerId,
                        players.FirstOrDefault(p => p.Id == x.FromPlayerId)?.Name ?? "", x.PowerPlay)).ToList() ?? []);
            playersAppliedPower.Add(playerAppliedPowerPlay);
        }

        return new GetAppliedPowerPlayResult(playersAppliedPower);
    }
}
