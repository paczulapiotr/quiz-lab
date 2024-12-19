
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Quiz.Common.CQRS;
using Quiz.Master.Persistance;
using Quiz.Master.Persistance.Models;
using Quiz.Master.Persistance.Models.MiniGames.AbcdCategories;

namespace Quiz.Master.Features.MiniGame.GetMiniGame;

public record GetMiniGameResult(string MiniGameId, MiniGameType MiniGameType, string? PlayerName, string? PlayerDeviceId, int Score, object? State = null);
public record GetMiniGameQuery(Guid GameId, string PlayerDeviceId) : IQuery<GetMiniGameResult>;

public class GetMiniGameHandler(IQuizRepository quizRepository) : IQueryHandler<GetMiniGameQuery, GetMiniGameResult>
{

    public async ValueTask<GetMiniGameResult?> HandleAsync(GetMiniGameQuery request, CancellationToken cancellationToken = default)
    {
        var activeGame = await quizRepository.Query<Persistance.Models.Game>()
            .Include(x => x.Players)
            .Include(x => x.MiniGames)
            .ThenInclude(x => x.MiniGameDefinition)
            .Where(x => x.Id == request.GameId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        var currentMiniGame = activeGame?.CurrentMiniGame;
        var player = activeGame?.Players.FirstOrDefault(x => x.DeviceId == request.PlayerDeviceId);
        var score = player?.Scores.Sum(x => x.Score) ?? 0;

        return currentMiniGame is null
            ? null
            : new GetMiniGameResult(
                currentMiniGame.Id.ToString(),
                currentMiniGame.MiniGameDefinition.Type,
                player?.Name,
                player?.DeviceId,
                score,
                StateSerializer(currentMiniGame.StateJsonData, currentMiniGame.MiniGameDefinition.Type));
    }


    private static object? StateSerializer(string stateJsonData, MiniGameType type)
    {
        return type switch
        {
            MiniGameType.AbcdWithCategories => string.IsNullOrWhiteSpace(stateJsonData) ? new AbcdWithCategoriesState() : JsonSerializer.Deserialize<AbcdWithCategoriesState>(stateJsonData),
            _ => null
        };
    }
}
