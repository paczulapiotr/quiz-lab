
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Quiz.Common.CQRS;
using Quiz.Master.Persistance;
using Quiz.Master.Persistance.Models;
using Quiz.Master.Persistance.Models.MiniGames.AbcdCategories;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetQuestion;

public record GetQuestionQuery(Guid GameId) : IQuery<GetQuestionResult>;
public record GetQuestionResult(string? QuestionId = null, string? Question = null, IEnumerable<Answer>? Answers = null);
public record Answer(string Id, string Text);

public class GetQuestionHandler(IQuizRepository quizRepository) : IQueryHandler<GetQuestionQuery, GetQuestionResult>
{
    public async ValueTask<GetQuestionResult?> HandleAsync(GetQuestionQuery request, CancellationToken cancellationToken = default)
    {
        var miniGame = await quizRepository.Query<MiniGameInstance>()
            .Include(x => x.MiniGameDefinition)
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

        return new GetQuestionResult(question?.Id, question?.Text, question?.Answers.Select(x => new Answer(x.Id, x.Text)));
    }
}
