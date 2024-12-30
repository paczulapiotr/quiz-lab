
using Microsoft.EntityFrameworkCore;
using Quiz.Common.CQRS;
using Quiz.Master.Core.Models;
using Quiz.Master.Persistance;


namespace Quiz.Master.Features.MiniGame.GetMiniGame;

public record GetScoresResult(string MiniGameId, MiniGameType MiniGameType, string? PlayerName, string? PlayerDeviceId, int MiniGameScore, int TotalScore, PlayerScore[] PlayerScores);
public record PlayerScore(string PlayerName, string PlayerDeviceId, int MiniGameScore, int TotalScore);
public record GetScoresQuery(Guid GameId, string DeviceId) : IQuery<GetScoresResult>;

public class GetScoresHandler(IQuizRepository quizRepository) : IQueryHandler<GetScoresQuery, GetScoresResult>
{
    public async ValueTask<GetScoresResult?> HandleAsync(GetScoresQuery request, CancellationToken cancellationToken = default)
    {
        var activeGame = await quizRepository.Query<Core.Models.Game>()
            .Include(x => x.Players).ThenInclude(x => x.Scores)
            .Include(x => x.MiniGames)
            .ThenInclude(x => x.MiniGameDefinition)
            .Where(x => x.Id == request.GameId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        var currentMiniGame = activeGame?.CurrentMiniGame;
        var players = activeGame?.Players;
        var player = players?.FirstOrDefault(x => x.DeviceId == request.DeviceId);

        if (currentMiniGame is null)
        {
            throw new InvalidOperationException("Mini game not found");
        }

        var (miniGameScore, totalScore) = GetPlayerScores(player, currentMiniGame.Id);

        return new GetScoresResult(
            currentMiniGame.Id.ToString(),
            currentMiniGame.MiniGameDefinition.Type,
            player?.Name,
            player?.DeviceId,
            miniGameScore,
            totalScore,
            players?.Select(x =>
            {
                var (playerMiniGameScore, playerTotalScore) = GetPlayerScores(x, currentMiniGame.Id);
                return new PlayerScore(x.Name, x.DeviceId, playerMiniGameScore, playerTotalScore);
            }).ToArray() ?? []);
    }

    private static (int miniGameScore, int totalScore) GetPlayerScores(Player? player, Guid miniGameId)
    {
        var totalScore = player?.Scores.Sum(x => x.Score) ?? 0;
        var miniGameScore = player?.Scores.FirstOrDefault(x => x.MiniGameInstanceId == miniGameId)?.Score ?? 0;

        return (miniGameScore, totalScore);
    }
}
