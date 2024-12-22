
using Microsoft.EntityFrameworkCore;
using Quiz.Common.CQRS;
using Quiz.Master.Persistance;
using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Features.MiniGame.GetMiniGame;

public record GetScoresResult(string MiniGameId, MiniGameType MiniGameType, string? PlayerName, string? PlayerDeviceId, int MiniGameScore, int TotalScore, PlayerScore[] PlayerScores);
public record PlayerScore(string PlayerName, string PlayerDeviceId, int MiniGameScore, int TotalScore);
public record GetScoresQuery(Guid GameId, string DeviceId) : IQuery<GetScoresResult>;

public class GetScoresHandler(IQuizRepository quizRepository) : IQueryHandler<GetScoresQuery, GetScoresResult>
{
    public async ValueTask<GetScoresResult?> HandleAsync(GetScoresQuery request, CancellationToken cancellationToken = default)
    {
        var activeGame = await quizRepository.Query<Persistance.Models.Game>()
            .Include(x => x.Players)
            .Include(x => x.MiniGames)
            .ThenInclude(x => x.MiniGameDefinition)
            .Where(x => x.Id == request.GameId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        var currentMiniGame = activeGame?.CurrentMiniGame;
        var players = activeGame?.Players;
        var player = players?.FirstOrDefault(x => x.DeviceId == request.DeviceId);
        var (miniGameScore, totalScore) = GetPlayerScores(activeGame!, request.DeviceId);

        if (currentMiniGame is null)
        {
            throw new InvalidOperationException("Mini game not found");
        }

        return new GetScoresResult(
            currentMiniGame.Id.ToString(),
            currentMiniGame.MiniGameDefinition.Type,
            player?.Name,
            player?.DeviceId,
            miniGameScore,
            totalScore,
            activeGame?.Players.Select(x =>
            {
                var (playerMiniGameScore, playerTotalScore) = GetPlayerScores(activeGame!, request.DeviceId);
                return new PlayerScore(x.Name, x.DeviceId, playerMiniGameScore, playerTotalScore);
            }).ToArray() ?? []);
    }

    private static (int miniGameScore, int totalScore) GetPlayerScores(Persistance.Models.Game game, string deviceId)
    {
        var player = game?.Players.FirstOrDefault(x => x.DeviceId == deviceId);
        var totalScore = player?.Scores.Sum(x => x.Score) ?? 0;
        var miniGameScore = player?.Scores.FirstOrDefault(x => x.MiniGameInstanceId == game!.CurrentMiniGame?.Id)?.Score ?? 0;

        return (miniGameScore, totalScore);
    }
}
