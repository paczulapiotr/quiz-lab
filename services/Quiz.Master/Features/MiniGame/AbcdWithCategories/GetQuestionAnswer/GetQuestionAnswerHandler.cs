
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Quiz.Common.CQRS;
using Quiz.Master.Persistance;
using Quiz.Master.Persistance.Models;
using Quiz.Master.Persistance.Models.MiniGames.AbcdCategories;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetQuestionAnswer;

public record GetQuestionAnswerQuery(Guid GameId, string DeviceId) : IQuery<GetQuestionAnswerResult>;
public record GetQuestionAnswerResult(string? AnswerId, PlayerAnswer[]? Answers, int Points = 0);
public record PlayerAnswer(string Id, string Text, bool IsCorrect, Player[] Players);
public record Player(string Id, string Name);

public class GetQuestionAnswerHandler(IQuizRepository quizRepository) : IQueryHandler<GetQuestionAnswerQuery, GetQuestionAnswerResult>
{

    public async ValueTask<GetQuestionAnswerResult?> HandleAsync(GetQuestionAnswerQuery request, CancellationToken cancellationToken = default)
    {
        var miniGame = await quizRepository.Query<MiniGameInstance>()
            .Include(x => x.MiniGameDefinition)
            .Include(x => x.Game).ThenInclude(x => x.Players)
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

        var playerAnswers = question?.Answers.Select(x
            => new PlayerAnswer(
                x.Id,
                x.Text,
                x.IsCorrect,
                answers.Where(a => a.AnswerId == x.Id).Select(a
                    => new Player(
                        a.DeviceId,
                        players.First(p => p.DeviceId == a.DeviceId).Name
                        )
                    ).ToArray())
            ).ToArray();

        var playerAnswer = answers.FirstOrDefault(x => x.DeviceId == request.DeviceId);
        var answerId = playerAnswer?.AnswerId;
        var points = playerAnswer?.Points ?? 0;

        return new GetQuestionAnswerResult(answerId, playerAnswers, points);
    }
}
