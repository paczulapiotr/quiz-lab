
using Quiz.Common.CQRS;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Storage;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetCategories;

public record GetCategoriesQuery(Guid GameId) : IQuery<GetCategoriesResult>;
public record GetCategoriesResult(IEnumerable<Category> Categories);
public record Category(string Id, string Text);

public class GetCategoriesHandler(IDatabaseStorage storage) : IQueryHandler<GetCategoriesQuery, GetCategoriesResult>
{
    public async ValueTask<GetCategoriesResult?> HandleAsync(GetCategoriesQuery request, CancellationToken cancellationToken = default)
    {
        var (state, definition) = await storage.FindCurrentMiniGameStateAndDefinitionAsync
            <AbcdWithCategoriesState, AbcdWithCategoriesDefinition>(request.GameId, cancellationToken);

        var categories = definition?.Rounds?.FirstOrDefault(x => x.Id == state?.CurrentRoundId)
            ?.Categories;

        if (categories is null)
        {
            throw new InvalidOperationException("Question not found");
        }

        return new GetCategoriesResult(categories.Select(x => new Category(x.Id, x.Name)));
    }
}
