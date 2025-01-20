using Quiz.Master.MiniGames.Models.AbcdCategories;

namespace Quiz.Master.MiniGames.Handlers.AbcdWithCategories.Logic;

public class PowerPlaySelector(IMiniGameEventService eventService) : CommonSelection<IMiniGameEventService.PowerPlaySelection, PowerPlaysDictionary>
{
    protected override async Task<(string? playerId, IMiniGameEventService.PowerPlaySelection?)> SelectOne(string gameId, CancellationToken cancellationToken)
    {
        var result = await eventService.GetPowerPlaySelection(gameId, cancellationToken);
        return (result?.PlayerId.ToString(), result);
    }

    protected override PowerPlaysDictionary UpdateSelectionState(IMiniGameEventService.PowerPlaySelection selection, PowerPlaysDictionary state)
    {
        if (state.ContainsKey(selection.PlayerId))
        {
            state[selection.TargetPlayerId].Add(new(selection.PlayerId, selection.PowerPlay));
        }
        else
        {
            state.Add(selection.TargetPlayerId, [new(selection.PlayerId, selection.PowerPlay)]);
        }

        return state;
    }
}
