using Quiz.Common.CQRS;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Master.Persistance;
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

        var miniGame = await storage.FindMiniGameAsync<AbcdWithCategoriesState>(game.CurrentMiniGameId.Value);
        
        if(miniGame.Type != MiniGameType.AbcdWithCategories) {
            throw new InvalidOperationException("Mini game is not of type AbcdWithCategories");
        }

        var miniGameDefinition = await storage.FindMiniGameDefinitionAsync<AbcdWithCategoriesDefinition>(miniGame.MiniGameDefinitionId);

        var categories = miniGameDefinition.Definition.Rounds?.FirstOrDefault(x => x.Id == miniGame.State.CurrentRoundId)
            ?.Categories ?? [];

        var players = game.Players;

        var selectedCategories = categories.Select(c => new SelectedCategory(
            c.Id,
            c.Name,
            c.Id == miniGame.State.CurrentCategoryId,
            miniGame.State.Rounds?.FirstOrDefault(x => x.RoundId == miniGame.State.CurrentRoundId)
                ?.SelectedCategories.FirstOrDefault(x => x.CategoryId == c.Id)?.PlayerIds
                .Select(playerId => new Player(playerId.ToString(), players?.FirstOrDefault(p => p.Id == playerId)?.Name ?? "")).ToArray() ?? []
        )).ToArray();

        return new GetSelectedCategoryResult(selectedCategories);
    }
}
