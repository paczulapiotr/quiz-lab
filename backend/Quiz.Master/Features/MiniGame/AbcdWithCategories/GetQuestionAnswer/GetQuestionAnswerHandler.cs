
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Quiz.Common.CQRS;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Storage;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetQuestionAnswer;

public record GetQuestionAnswerQuery(Guid GameId, string DeviceId) : IQuery<GetQuestionAnswerResult>;
public record GetQuestionAnswerResult(PlayerResult? Answer, AnswerResult[]? Answers, PlayerResult[]? Players);
public record AnswerResult(string Id, string Text, bool IsCorrect);
public record PlayerResult(string? Id, string? Name, string? AnswerId, int AnswerPoints, int RoundPoints);

public class GetQuestionAnswerHandler(IDatabaseStorage storage) : IQueryHandler<GetQuestionAnswerQuery, GetQuestionAnswerResult>
{

    public async ValueTask<GetQuestionAnswerResult?> HandleAsync(GetQuestionAnswerQuery request, CancellationToken cancellationToken = default)
    {
        var game = await storage.FindGameAsync(request.GameId, cancellationToken);
        var miniGame = await storage.FindMiniGameAsync<AbcdWithCategoriesState>(game.CurrentMiniGameId!.Value, cancellationToken);
        var miniGameDefinition = await storage.FindMiniGameDefinitionAsync<AbcdWithCategoriesDefinition>(miniGame.MiniGameDefinitionId, cancellationToken);
        if (miniGame == null)
        {
            throw new InvalidOperationException("Mini game not found");
        }

        var definition = miniGameDefinition.Definition;
        var state = miniGame.State;
        var players = game.Players;
        var question = definition.Rounds?.FirstOrDefault(x => x.Id == state.CurrentRoundId)
            ?.Categories?.FirstOrDefault(x => x.Id == state.CurrentCategoryId)
            ?.Questions.FirstOrDefault(x => x.Id == state.CurrentQuestionId);

        var answers = state.Rounds?.FirstOrDefault(x => x.RoundId == state.CurrentRoundId)?.Answers ?? [];
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

        var playerAnswer = playerResults.FirstOrDefault(x => x.Id == request.DeviceId);

        return new GetQuestionAnswerResult(playerAnswer, answerResults, playerResults);
    }
}
