
using Quiz.Common.CQRS;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models;
using Quiz.Storage;

namespace Quiz.Master.Features.MiniGame.GetMiniGame;

public record GetMiniGameResult(
    Guid MiniGameId, 
    MiniGameType MiniGameType, 
    string? PlayerName, 
    string? PlayerId, 
    string? PlayerDeviceId, 
    int Score, 
    BaseState? State = null, 
    BaseDefinition? Definition = null);
public record GetMiniGameQuery(Guid GameId, string PlayerDeviceId) : IQuery<GetMiniGameResult>;

public class GetMiniGameHandler(IDatabaseStorage storage) : IQueryHandler<GetMiniGameQuery, GetMiniGameResult>
{

    public async ValueTask<GetMiniGameResult?> HandleAsync(GetMiniGameQuery request, CancellationToken cancellationToken = default)
    {
        var game = await storage.FindGameAsync(request.GameId, cancellationToken);

        if(game.CurrentMiniGameId is null)
        {
            return null;
        }

        var miniGame = await storage.FindMiniGameAsync(game.CurrentMiniGameId.Value, cancellationToken);
        var definition = await storage.FindMiniGameDefinitionAsync(miniGame.MiniGameDefinitionId, cancellationToken);
        
        if (miniGame == null)
        {
            return null;
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
                player?.Id.ToString(),
                player?.DeviceId,
                score,
                miniGame.State.As<BaseState>(),
                definition.Definition.As<BaseDefinition>());
    }
}
