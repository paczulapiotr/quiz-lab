using State = Quiz.Master.MiniGames.Models.AbcdCategories.AbcdWithCategoriesState;

namespace Quiz.Master.MiniGames.Handlers.AbcdWithCategories.Logic;

public class AnswerSelector(IMiniGameEventService eventService, string correctAnswerId, int maxPoints, int minPoints, int pointsDecrement) : CommonSelection<IMiniGameEventService.AnswerSelection, List<State.RoundAnswer>>
{
    protected override async Task<(string? playerId, IMiniGameEventService.AnswerSelection?)> SelectOne(string gameId, CancellationToken cancellationToken)
    {
        var result = await eventService.GetQuestionAnswerSelection(gameId, cancellationToken);
        return (result?.PlayerId.ToString(), result);
    }

    protected override List<State.RoundAnswer>? UpdateSelectionState(IMiniGameEventService.AnswerSelection selection, List<State.RoundAnswer>? state)
    {
        if (state is null)
        {
            state = new List<State.RoundAnswer>();
        }
        
        state.Add(new State.RoundAnswer
        {
            PlayerId = selection.PlayerId,
            AnswerId = selection.AnswerId,
            IsCorrect = selection.AnswerId == correctAnswerId,
            Timestamp = selection.Timestamp,
            Points = 0,
        });

        var correctAnswerCount = 0;

        foreach (var ans in state.OrderByDescending(x => x.Timestamp))
        {
            if (ans.IsCorrect)
            {
                ans.Points = Math.Max(maxPoints - (correctAnswerCount * pointsDecrement), minPoints);
                correctAnswerCount++;
            }
        }

        return state;
    }
}
