
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Quiz.Common.CQRS;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Master.Persistance;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetQuestionAnswer;

public record GetQuestionAnswerQuery(Guid GameId, string DeviceId) : IQuery<GetQuestionAnswerResult>;
public record GetQuestionAnswerResult(PlayerResult? Answer, AnswerResult[]? Answers, PlayerResult[]? Players);
public record AnswerResult(string Id, string Text, bool IsCorrect);
public record PlayerResult(string? Id, string? Name, string? AnswerId, int AnswerPoints, int RoundPoints);

public class GetQuestionAnswerHandler(IQuizRepository quizRepository) : IQueryHandler<GetQuestionAnswerQuery, GetQuestionAnswerResult>
{

    public async ValueTask<GetQuestionAnswerResult?> HandleAsync(GetQuestionAnswerQuery request, CancellationToken cancellationToken = default)
    {
        var miniGame = await quizRepository.Query<MiniGameInstance>()
            .Include(x => x.MiniGameDefinition)
            .Include(x => x.Game).ThenInclude(x => x.Players).ThenInclude(x => x.Scores)
            .Where(x => x.Game.Id == request.GameId && x.MiniGameDefinition.Type == MiniGameType.AbcdWithCategories)
            .FirstOrDefaultAsync();

        if (miniGame == null)
        {
            throw new InvalidOperationException("Mini game not found");
        }

        var state = JsonSerializer.Deserialize<AbcdWithCategoriesState>(miniGame.StateJsonData);
        var definition = JsonSerializer.Deserialize<AbcdWithCategoriesDefinition>(miniGame.MiniGameDefinition.DefinitionJsonData);

        if (state is null || definition is null)
        {
            throw new InvalidOperationException("Mini game state or definition not found");
        }

        var question = definition.Rounds?.FirstOrDefault(x => x.Id == state.CurrentRoundId)
            ?.Categories?.FirstOrDefault(x => x.Id == state.CurrentCategoryId)
            ?.Questions.FirstOrDefault(x => x.Id == state.CurrentQuestionId);

        var answers = state.Rounds?.FirstOrDefault(x => x.RoundId == state.CurrentRoundId)?.Answers ?? [];
        var players = miniGame.Game.Players;
        var answerResults = question?.Answers.Select(x
            => new AnswerResult(
                x.Id,
                x.Text,
                x.IsCorrect
                )
            ).ToArray() ?? [];

        var playerResults = players.Select(x =>
        {

            var ans = answers.FirstOrDefault(a => a.DeviceId == x.DeviceId);
            return new PlayerResult(
            x.DeviceId,
            players.FirstOrDefault(p => p.DeviceId == x.DeviceId)?.Name,
            ans?.AnswerId,
            ans?.Points ?? 0,
            x.Scores.Sum(s => s.Score)
            );
        }).ToArray();

        var playerAnswer = playerResults.FirstOrDefault(x => x.Id == request.DeviceId);

        return new GetQuestionAnswerResult(playerAnswer, answerResults, playerResults);
    }
}
