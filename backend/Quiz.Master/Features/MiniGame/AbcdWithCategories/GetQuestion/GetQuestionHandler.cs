
using Quiz.Common.CQRS;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Storage;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetQuestion;

public record GetQuestionQuery(Guid GameId) : IQuery<GetQuestionResult>;
public record GetQuestionResult(string? QuestionId = null, string? Question = null, IEnumerable<Answer>? Answers = null);
public record Answer(string Id, string Text);

public class GetQuestionHandler(IDatabaseStorage storage) : IQueryHandler<GetQuestionQuery, GetQuestionResult>
{
    public async ValueTask<GetQuestionResult?> HandleAsync(GetQuestionQuery request, CancellationToken cancellationToken = default)
    {
        var game = await storage.FindGameAsync(request.GameId, cancellationToken);
        var miniGame = await storage.FindMiniGameAsync<AbcdWithCategoriesState>(game.CurrentMiniGameId!.Value, cancellationToken);
        var miniGameDefinition = await storage.FindMiniGameDefinitionAsync<AbcdWithCategoriesDefinition>(miniGame.MiniGameDefinitionId, cancellationToken);

        if (miniGame == null)
        {
            throw new InvalidOperationException("Mini game not found");
        }

        var question = miniGameDefinition.Definition.Rounds?.FirstOrDefault(x => x.Id == miniGame.State.CurrentRoundId)
            ?.Categories?.FirstOrDefault(x => x.Id == miniGame.State.CurrentCategoryId)
            ?.Questions.FirstOrDefault(x => x.Id == miniGame.State.CurrentQuestionId);

        return new GetQuestionResult(question?.Id, question?.Text, question?.Answers.Select(x => new Answer(x.Id, x.Text)));
    }
}
