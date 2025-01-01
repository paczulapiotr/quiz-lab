
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Quiz.Common.CQRS;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Master.Persistance;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetAppliedPowerPlay;

public record GetAppliedPowerPlayQuery(Guid GameId, string DeviceId) : IQuery<GetAppliedPowerPlayResult>;
public record GetAppliedPowerPlayResult(IEnumerable<PlayersAppliedPower> players);
public record PlayersAppliedPower(string PlayerId, string PlayerName, List<AppliedPowerPlay> PowerPlays);
public record AppliedPowerPlay(string PlayerId, string PlayerName, PowerPlay PowerPlay);

public class GetAppliedPowerPlayHandler(IQuizRepository quizRepository) : IQueryHandler<GetAppliedPowerPlayQuery, GetAppliedPowerPlayResult>
{

    public async ValueTask<GetAppliedPowerPlayResult?> HandleAsync(GetAppliedPowerPlayQuery request, CancellationToken cancellationToken = default)
    {
        var miniGame = await quizRepository.Query<MiniGameInstance>()
            .Include(x => x.MiniGameDefinition)
            .Include(x => x.Game).ThenInclude(x => x.Players)
            .Where(x => x.Game.Id == request.GameId && x.MiniGameDefinition.Type == MiniGameType.AbcdWithCategories)
            .FirstOrDefaultAsync();

        if (miniGame == null)
        {
            throw new InvalidOperationException("Mini game not found");
        }

        var state = JsonSerializer.Deserialize<AbcdWithCategoriesState>(miniGame.StateJsonData);

        if (state is null)
        {
            throw new InvalidOperationException("Mini game state not found");
        }

        var players = miniGame.Game.Players;

        var playersToMap = string.IsNullOrWhiteSpace(request.DeviceId)
            ? players
            : players.Where(x => x.DeviceId == request.DeviceId).ToList();

        var powerPlays = state.Rounds.LastOrDefault()?.PowerPlays;

        var playersAppliedPower = new List<PlayersAppliedPower>();
        foreach (var player in playersToMap)
        {
            var powerPlaysForPlayer = powerPlays?.FirstOrDefault(x => x.Key == player.DeviceId).Value;
            var playerAppliedPowerPlay = new PlayersAppliedPower(
                player.DeviceId,
                player.Name,
                powerPlaysForPlayer?.Select(x
                    => new AppliedPowerPlay(
                        x.SourceDeviceId,
                        players.FirstOrDefault(p => p.DeviceId == x.SourceDeviceId)?.Name ?? "", x.PowerPlay)).ToList() ?? []);
            playersAppliedPower.Add(playerAppliedPowerPlay);
        }

        return new GetAppliedPowerPlayResult(playersAppliedPower);
    }
}
