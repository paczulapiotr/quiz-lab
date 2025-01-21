using Carter;
using Quiz.Master.Extensions;
using Quiz.Master.MiniGames.Models.MusicGuess;
using Quiz.Storage;

namespace Quiz.Master.Features.MiniGame.MusicGuess.GetQuestionAnswer;

public class GetQuestionAnswerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/game/{gameId}/mini-game/music/question-answer", async (Guid gameId, IHttpContextAccessor httpContextAccessor, IDatabaseStorage storage, CancellationToken cancellationToken = default) =>
        {
            var deviceId = httpContextAccessor.GetDeviceId();
            var game = await storage.FindGameAsync(gameId, cancellationToken);
            var miniGame = await storage.FindMiniGameAsync(game.CurrentMiniGameId!.Value, cancellationToken);
            var miniGameDefinition = await storage.FindMiniGameDefinitionAsync(miniGame.MiniGameDefinitionId, cancellationToken);

            if (miniGame == null)
            {
                throw new InvalidOperationException("Mini game not found");
            }

            var state = miniGame.State.As<MusicGuessState>();
            var definition = miniGameDefinition.Definition.As<MusicGuessDefinition>();

            var players = game.Players;
            var question = definition?.Rounds?.FirstOrDefault(x => x.Id == state?.CurrentRoundId)
                ?.Categories?.FirstOrDefault(x => x.Id == state?.CurrentCategoryId)
                ?.Questions.FirstOrDefault(x => x.Id == state?.CurrentQuestionId);

            var answers = state?.Rounds?.FirstOrDefault(x => x.RoundId == state.CurrentRoundId)?.Answers ?? [];
            var answerResults = question?.Answers.Select(x
                => new AnswerResult(
                    x.Id,
                    x.Text,
                    x.IsCorrect
                    )
                ).ToArray() ?? [];

            var playerResults = players.Select(x =>
            {
                var ans = answers.FirstOrDefault(a => a.PlayerId == x.Id);
                return new PlayerResult(
                    x.DeviceId,
                    players.FirstOrDefault(p => p.DeviceId == x.DeviceId)?.Name,
                    ans?.AnswerId,
                    ans?.Points ?? 0,
                    miniGame.PlayerScores.FirstOrDefault(ps => ps.PlayerId == x.Id)?.Score ?? 0
                );
            }).ToArray();

            var playerAnswer = playerResults.FirstOrDefault(x => x.Id == deviceId);

            var result = new GetQuestionAnswerResult(playerAnswer, answerResults, playerResults);

            return Results.Ok(result);
        })
        .WithName("MusicGetQuestionAnswer")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Question Answer")
        .WithDescription("Get Question Answer")
        .WithTags("MusicGuess");
    }
}

internal record GetQuestionAnswerResult(PlayerResult? Answer, AnswerResult[]? Answers, PlayerResult[]? Players);
internal record AnswerResult(string Id, string Text, bool IsCorrect);
internal record PlayerResult(string? Id, string? Name, string? AnswerId, int AnswerPoints, int RoundPoints);