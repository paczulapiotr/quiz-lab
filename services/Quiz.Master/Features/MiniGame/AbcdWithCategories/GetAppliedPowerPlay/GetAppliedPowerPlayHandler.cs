
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Quiz.Common.CQRS;
using Quiz.Master.Persistance;
using Quiz.Master.Persistance.Models;
using Quiz.Master.Persistance.Models.MiniGames.AbcdCategories;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetAppliedPowerPlay;

public record GetAppliedPowerPlayQuery(Guid GameId, string DeviceId) : IQuery<GetAppliedPowerPlayResult>;
public record GetAppliedPowerPlayResult(IEnumerable<AppliedPowerPlay> powerPlays);
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

        var players = miniGame.Game.Players.Where(x => x.DeviceId != request.DeviceId);

        var powerPlays = state.Rounds.LastOrDefault()?.PowerPlays
            .FirstOrDefault(x => x.Key == request.DeviceId).Value;

        return new GetAppliedPowerPlayResult((powerPlays ?? []).Select(x
            => new AppliedPowerPlay(
                x.SourceDeviceId,
                players.FirstOrDefault(p => p.DeviceId == x.SourceDeviceId)?.Name ?? "",
                x.PowerPlay)));
    }
}
