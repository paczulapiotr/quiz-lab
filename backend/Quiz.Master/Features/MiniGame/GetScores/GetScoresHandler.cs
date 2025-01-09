
using Quiz.Common.CQRS;
using Quiz.Master.Core.Models;
using Quiz.Storage;


namespace Quiz.Master.Features.MiniGame.GetMiniGame;

public record GetScoresResult(string MiniGameId, MiniGameType MiniGameType, string? PlayerName, Guid? PlayerId, int MiniGameScore, int TotalScore, PlayerScore[] PlayerScores);
public record PlayerScore(string PlayerName, string PlayerDeviceId, int MiniGameScore, int TotalScore);
public record GetScoresQuery(Guid GameId, string DeviceId) : IQuery<GetScoresResult>;

public class GetScoresHandler(IDatabaseStorage storage) : IQueryHandler<GetScoresQuery, GetScoresResult>
{
    public async ValueTask<GetScoresResult?> HandleAsync(GetScoresQuery request, CancellationToken cancellationToken = default)
    {
        var activeGame = await storage.FindGameAsync(request.GameId, cancellationToken);
        var currentMiniGameId = activeGame?.CurrentMiniGameId;
        if (currentMiniGameId is null || activeGame is null)
        {
            throw new InvalidOperationException("Mini game not found");
        }

        var currentMiniGame = await storage.FindMiniGameAsync<MiniGameStateData>(currentMiniGameId.Value, cancellationToken);
        var gameDefinition = await storage.FindGameDefinitionAsync(activeGame.GameDefinitionId, cancellationToken);

        var players = activeGame?.Players;
        var player = players?.FirstOrDefault(x => x.DeviceId == request.DeviceId);

        var playerScores = await storage.ListPlayerScores(request.GameId, cancellationToken);
        var (miniGameScore, totalScore) = GetPlayerScores(playerScores, player, currentMiniGameId.Value);

        return new GetScoresResult(
            currentMiniGame.Id.ToString(),
            currentMiniGame.Type,
            player?.Name,
            player?.Id,
            miniGameScore,
            totalScore,
            players?.Select(x =>
            {
                var (playerMiniGameScore, playerTotalScore) = GetPlayerScores(playerScores, x, currentMiniGame.Id);
                return new PlayerScore(x.Name, x.DeviceId, playerMiniGameScore, playerTotalScore);
            }).ToArray() ?? []);
    }

    private static (int miniGameScore, int totalScore) GetPlayerScores(IEnumerable<(Player player, Dictionary<Guid, int> scores)> playerScores, Player? player, Guid miniGameId)
    {
        var (_, playerScore) = playerScores.FirstOrDefault(x => x.player.Id == player?.Id);
        var miniGameScore = playerScore?.Where(x => x.Key == miniGameId).Sum(x => x.Value) ?? 0;
        var totalScore = playerScores.Sum(x => x.scores.Sum(y => y.Value));

        return (miniGameScore, totalScore);
    }
}
