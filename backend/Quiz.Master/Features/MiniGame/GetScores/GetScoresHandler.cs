
using Quiz.Common.CQRS;
using Quiz.Master.Core.Models;
using Quiz.Storage;


namespace Quiz.Master.Features.MiniGame.GetMiniGame;

public record GetScoresResult(string? MiniGameId, MiniGameType? MiniGameType, string? PlayerName, Guid? PlayerId, int MiniGameScore, int TotalScore, PlayerScore[] PlayerScores);
public record PlayerScore(string PlayerName, string PlayerDeviceId, int MiniGameScore, int TotalScore);
public record GetScoresQuery(Guid GameId, string DeviceId) : IQuery<GetScoresResult>;

public class GetScoresHandler(IDatabaseStorage storage) : IQueryHandler<GetScoresQuery, GetScoresResult>
{
    public async ValueTask<GetScoresResult?> HandleAsync(GetScoresQuery request, CancellationToken cancellationToken = default)
    {
        var game = await storage.FindGameAsync(request.GameId, cancellationToken);
        if (game is null)
        {
            return new GetScoresResult(null, null, null, null, 0, 0, Array.Empty<PlayerScore>());
        }
        
        var currentMiniGameId = game?.CurrentMiniGameId;
        var currentMiniGame = currentMiniGameId.HasValue ? await storage.FindMiniGameAsync(currentMiniGameId.Value, cancellationToken) : null;
        var gameDefinition = await storage.FindGameDefinitionAsync(game!.GameDefinitionId, cancellationToken);

        var players = game?.Players;
        var player = players?.FirstOrDefault(x => x.DeviceId == request.DeviceId);

        var playerScores = await storage.ListPlayerScores(request.GameId, cancellationToken);
        var (miniGameScore, totalScore) = GetPlayerScores(playerScores, player, currentMiniGameId);

        return new GetScoresResult(
            currentMiniGameId?.ToString(),
            currentMiniGame?.Type,
            player?.Name,
            player?.Id,
            miniGameScore,
            totalScore,
            players?.Select(x =>
            {
                var (playerMiniGameScore, playerTotalScore) = GetPlayerScores(playerScores, x, currentMiniGameId);
                return new PlayerScore(x.Name, x.DeviceId, playerMiniGameScore, playerTotalScore);
            }).ToArray() ?? []);
    }

    private static (int miniGameScore, int totalScore) GetPlayerScores(IEnumerable<(Player player, Dictionary<Guid, int> scores)> playerScores, Player? player, Guid? miniGameId)
    {
        var (_, playerScore) = playerScores.FirstOrDefault(x => x.player.Id == player?.Id);
        var miniGameScore = miniGameId.HasValue ? playerScore?.Where(x => x.Key == miniGameId).Sum(x => x.Value) ?? 0 : 0;
        var totalScore = playerScore?.Sum(x => x.Value) ?? 0;

        return (miniGameScore, totalScore);
    }
}
