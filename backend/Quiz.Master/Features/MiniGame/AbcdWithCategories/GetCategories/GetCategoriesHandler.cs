
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
        var game = await storage.FindGameAsync(request.GameId, cancellationToken);
        var miniGame = await storage.FindMiniGameAsync<AbcdWithCategoriesState>(game.CurrentMiniGameId!.Value, cancellationToken);
        var miniGameDefinition = await storage.FindMiniGameDefinitionAsync<AbcdWithCategoriesDefinition>(miniGame.MiniGameDefinitionId, cancellationToken);

        if (miniGame == null)
        {
            throw new InvalidOperationException("Mini game not found");
        }

        var categories = miniGameDefinition.Definition.Rounds?.FirstOrDefault(x => x.Id == miniGame.State.CurrentRoundId)
            ?.Categories;

        if (categories is null)
        {
            throw new InvalidOperationException("Question not found");
        }

        return new GetCategoriesResult(categories.Select(x => new Category(x.Id, x.Name)));
    }
}
