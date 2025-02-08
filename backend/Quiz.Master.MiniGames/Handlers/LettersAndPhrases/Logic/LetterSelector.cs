namespace Quiz.Master.MiniGames.Handlers.LettersAndPhrases.Logic;

public class LetterSelector(IMiniGameEventService eventService) : CommonSelection<IMiniGameEventService.AnswerSelection, IMiniGameEventService.AnswerSelection>
{
    protected override async Task<(string? playerId, IMiniGameEventService.AnswerSelection?)> SelectOne(string gameId, CancellationToken cancellationToken)
    {
        var answer = await eventService.GetLetterSelection(gameId, cancellationToken);
        
        return (answer?.PlayerId.ToString(), answer);
    }

    protected override IMiniGameEventService.AnswerSelection? UpdateSelectionState(IMiniGameEventService.AnswerSelection selection, IMiniGameEventService.AnswerSelection? state)
    {
        return selection;
    }
}
