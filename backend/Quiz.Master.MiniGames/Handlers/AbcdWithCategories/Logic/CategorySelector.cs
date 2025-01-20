using State = Quiz.Master.MiniGames.Models.AbcdCategories.AbcdWithCategoriesState;

namespace Quiz.Master.MiniGames.Handlers.AbcdWithCategories.Logic;

public class CategorySelector(IMiniGameEventService eventService, IEnumerable<string> categoryIds) : CommonSelection<IMiniGameEventService.CategorySelection, List<State.SelectedCategory>>
{
    protected override async Task<(string? playerId, IMiniGameEventService.CategorySelection?)> SelectOne(string gameId, CancellationToken cancellationToken)
    {
        var category = await eventService.GetCategorySelection(gameId, cancellationToken);
        if (!categoryIds.Contains(category?.CategoryId))
        {
            return (null, null);
        }

        return (category?.PlayerId.ToString(), category);
    }

    protected override List<State.SelectedCategory> UpdateSelectionState(IMiniGameEventService.CategorySelection selection, List<State.SelectedCategory> state)
    {
        var stateCat = state.FirstOrDefault(x => x.CategoryId == selection.CategoryId);
        if (stateCat == null)
        {
            stateCat = new State.SelectedCategory { CategoryId = selection.CategoryId, PlayerIds = [selection.PlayerId] };
            state.Add(stateCat);
        }
        else
        {
            stateCat.PlayerIds.Add(selection.PlayerId);
        }

        return state;
    }
}
