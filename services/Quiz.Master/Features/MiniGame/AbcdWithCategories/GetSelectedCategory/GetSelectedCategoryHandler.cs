
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Quiz.Common.CQRS;
using Quiz.Master.Persistance;
using Quiz.Master.Persistance.Models;
using Quiz.Master.Persistance.Models.MiniGames.AbcdCategories;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetSelectedCategory;

public record GetSelectedCategoryQuery(Guid GameId) : IQuery<GetSelectedCategoryResult>;
public record GetSelectedCategoryResult(SelectedCategory[] Categories);
public record SelectedCategory(string Id, string Text, bool IsSelected, Player[] Players);
public record Player(string Id, string Name);

public class GetSelectedCategoryHandler(IQuizRepository quizRepository) : IQueryHandler<GetSelectedCategoryQuery, GetSelectedCategoryResult>
{

    public async ValueTask<GetSelectedCategoryResult?> HandleAsync(GetSelectedCategoryQuery request, CancellationToken cancellationToken = default)
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

        var categories = definition.Rounds?.FirstOrDefault(x => x.Id == state.CurrentRoundId)
            ?.Categories ?? [];

        var players = miniGame.Game.Players;

        var selectedCategories = categories.Select(c => new SelectedCategory(
            c.Id,
            c.Name,
            c.Id == state.CurrentCategoryId,
            state.Rounds?.FirstOrDefault(x => x.RoundId == state.CurrentRoundId)
                ?.SelectedCategories.FirstOrDefault(x => x.CategoryId == c.Id)?.DeviceIds
                .Select(dId => new Player(dId, players?.FirstOrDefault(p => p.DeviceId == dId)?.Name ?? "")).ToArray() ?? []
        )).ToArray();

        return new GetSelectedCategoryResult(selectedCategories);
    }
}
