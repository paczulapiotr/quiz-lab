using Quiz.Master.MiniGames.Models.AbcdCategories;

namespace Quiz.Master.MiniGames.Handlers.AbcdWithCategories.Logic;

public class PowerPlaySelector(IMiniGameEventService eventService) : CommonSelection<IMiniGameEventService.PowerPlaySelection, PowerPlaysDictionary>
{
    protected override async Task<(string? playerId, IMiniGameEventService.PowerPlaySelection?)> SelectOne(string gameId, CancellationToken cancellationToken)
    {
        var result = await eventService.GetPowerPlaySelection(gameId, cancellationToken);
        return (result?.PlayerId.ToString(), result);
    }

    protected override PowerPlaysDictionary? UpdateSelectionState(IMiniGameEventService.PowerPlaySelection selection, PowerPlaysDictionary? state)
    {
        if (state is null)
        {
            state = new PowerPlaysDictionary();
        }
        
        if (state.ContainsKey(selection.TargetPlayerId.ToString()))
        {
            state[selection.TargetPlayerId.ToString()].Add(new(selection.PlayerId.ToString(), selection.PowerPlay));
        }
        else
        {
            state.Add(selection.TargetPlayerId.ToString(), [new(selection.PlayerId.ToString(), selection.PowerPlay)]);
        }

        return state;
    }
}
