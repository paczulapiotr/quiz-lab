using Quiz.Common.CQRS;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Storage;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetSelectedCategory;

public record GetSelectedCategoryQuery(Guid GameId) : IQuery<GetSelectedCategoryResult>;
public record GetSelectedCategoryResult(SelectedCategory[] Categories);
public record SelectedCategory(string Id, string Text, bool IsSelected, Player[] Players);
public record Player(string Id, string Name);

public class GetSelectedCategoryHandler(IDatabaseStorage storage) : IQueryHandler<GetSelectedCategoryQuery, GetSelectedCategoryResult>
{

    public async ValueTask<GetSelectedCategoryResult?> HandleAsync(GetSelectedCategoryQuery request, CancellationToken cancellationToken = default)
    {
        var game = await storage.FindGameAsync(request.GameId, cancellationToken);

        if(game.CurrentMiniGameId is null) {
            throw new InvalidOperationException("Game has no current mini game");
        }   

        var miniGame = await storage.FindMiniGameAsync(game.CurrentMiniGameId.Value);
        var miniGameDefinition = await storage.FindMiniGameDefinitionAsync(miniGame.MiniGameDefinitionId);

        var state = miniGame.State.As<AbcdWithCategoriesState>();
        var definition = miniGameDefinition.Definition.As<AbcdWithCategoriesDefinition>();

        var categories = definition?.Rounds?.FirstOrDefault(x => x.Id == state?.CurrentRoundId)
            ?.Categories ?? [];

        var players = game.Players;

        var selectedCategories = categories.Select(c => new SelectedCategory(
            c.Id,
            c.Name,
            c.Id == state?.CurrentCategoryId,
            state?.Rounds?.FirstOrDefault(x => x.RoundId == state?.CurrentRoundId)
                ?.SelectedCategories.FirstOrDefault(x => x.CategoryId == c.Id)?.PlayerIds
                .Select(playerId => new Player(playerId.ToString(), players?.FirstOrDefault(p => p.Id == playerId)?.Name ?? "")).ToArray() ?? []
        )).ToArray();

        return new GetSelectedCategoryResult(selectedCategories);
    }
}
