
using Quiz.Common.CQRS;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Storage;

namespace Quiz.Master.Features.MiniGame.GetMiniGame;

public record GetMiniGameResult(Guid MiniGameId, MiniGameType MiniGameType, string? PlayerName, string? PlayerDeviceId, int Score, object? State = null);
public record GetMiniGameQuery(Guid GameId, string PlayerDeviceId) : IQuery<GetMiniGameResult>;

public class GetMiniGameHandler(IDatabaseStorage storage) : IQueryHandler<GetMiniGameQuery, GetMiniGameResult>
{

    public async ValueTask<GetMiniGameResult?> HandleAsync(GetMiniGameQuery request, CancellationToken cancellationToken = default)
    {
        var game = await storage.FindGameAsync(request.GameId, cancellationToken);
        var miniGame = await storage.FindMiniGameAsync<AbcdWithCategoriesState>(game.CurrentMiniGameId!.Value, cancellationToken);
        var miniGameDefinition = await storage.FindMiniGameDefinitionAsync<AbcdWithCategoriesDefinition>(miniGame.MiniGameDefinitionId, cancellationToken);

        if (miniGame == null)
        {
            throw new InvalidOperationException("Mini game not found");
        }

        var currentMiniGame = game?.CurrentMiniGameId;
        var player = game?.Players.FirstOrDefault(x => x.DeviceId == request.PlayerDeviceId);
        var score = miniGame.PlayerScores.FirstOrDefault(x => x.PlayerId == player?.Id)?.Score ?? 0;

        return currentMiniGame is null
            ? null
            : new GetMiniGameResult(
                miniGame.Id,
                miniGame.Type,
                player?.Name,
                player?.DeviceId,
                score,
                miniGame.State);
    }
}
