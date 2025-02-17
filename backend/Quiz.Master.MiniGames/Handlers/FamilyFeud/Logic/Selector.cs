namespace Quiz.Master.MiniGames.Handlers.FamilyFeud.Logic;

public class Selector(IMiniGameEventService eventService) : CommonSelection<IMiniGameEventService.AnswerSelection, IMiniGameEventService.AnswerSelection>
{
    protected override async Task<(string? playerId, IMiniGameEventService.AnswerSelection?)> SelectOne(string gameId, CancellationToken cancellationToken)
    {
        var answer = await eventService.GetAnswer(gameId, cancellationToken);

        return (answer?.PlayerId.ToString(), answer);
    }

    protected override IMiniGameEventService.AnswerSelection? UpdateSelectionState(IMiniGameEventService.AnswerSelection selection, IMiniGameEventService.AnswerSelection? state)
    {
        return selection;
    }
}
