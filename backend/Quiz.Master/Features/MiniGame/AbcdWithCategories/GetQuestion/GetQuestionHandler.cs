
using Quiz.Common.CQRS;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Storage;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetQuestion;

public record GetQuestionQuery(Guid GameId) : IQuery<GetQuestionResult>;
public record GetQuestionResult(string? QuestionId = null, string? Question = null, string? AudioUrl = null,  IEnumerable<Answer>? Answers = null);
public record Answer(string Id, string Text);

public class GetQuestionHandler(IDatabaseStorage storage) : IQueryHandler<GetQuestionQuery, GetQuestionResult>
{
    public async ValueTask<GetQuestionResult?> HandleAsync(GetQuestionQuery request, CancellationToken cancellationToken = default)
    {
        var game = await storage.FindGameAsync(request.GameId, cancellationToken);
        var miniGame = await storage.FindMiniGameAsync(game.CurrentMiniGameId!.Value, cancellationToken);
        var miniGameDefinition = await storage.FindMiniGameDefinitionAsync(miniGame.MiniGameDefinitionId, cancellationToken);
        var state = miniGame.State.As<AbcdWithCategoriesState>();
        var definition = miniGameDefinition.Definition.As<AbcdWithCategoriesDefinition>();

        if (miniGame == null)
        {
            throw new InvalidOperationException("Mini game not found");
        }

        var question = definition?.Rounds?.FirstOrDefault(x => x.Id == state?.CurrentRoundId)
            ?.Categories?.FirstOrDefault(x => x.Id == state?.CurrentCategoryId)
            ?.Questions.FirstOrDefault(x => x.Id == state?.CurrentQuestionId);

        return new GetQuestionResult(question?.Id, question?.Text, question?.AudioUrl, question?.Answers.Select(x => new Answer(x.Id, x.Text)));
    }
}
