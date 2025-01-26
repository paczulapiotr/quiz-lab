using State = Quiz.Master.MiniGames.Models.MusicGuess.MusicGuessState;

namespace Quiz.Master.MiniGames.Handlers.MusicGuess.Logic;

public class AnswerSelector(IMiniGameEventService eventService, IEnumerable<string> answerIds, string correctAnswerId, int maxPoints, int minPoints, int pointsDecrement) : CommonSelection<IMiniGameEventService.AnswerSelection, List<State.RoundAnswer>>
{
    protected override async Task<(string? playerId, IMiniGameEventService.AnswerSelection?)> SelectOne(string gameId, CancellationToken cancellationToken)
    {
        var answer = await eventService.GetQuestionAnswerSelection(gameId, cancellationToken);
        if (!answerIds.Contains(answer?.AnswerId))
        {
            return (null, null);
        }

        return (answer?.PlayerId.ToString(), answer);
    }

    protected override List<State.RoundAnswer> UpdateSelectionState(IMiniGameEventService.AnswerSelection selection, List<State.RoundAnswer> state)
    {
        state.Add(
            new State.RoundAnswer
            {
                AnswerId = selection.AnswerId,
                PlayerId = selection.PlayerId,
                Timestamp = selection.Timestamp,
                IsCorrect = selection.AnswerId == correctAnswerId,
                Points = 0
            });

        var correctAnswerCount = 0;
        var answers = state.OrderBy(x => x.Timestamp).ToList();
        foreach (var ans in answers) {
            if(ans.IsCorrect) {
                ans.Points = Math.Max(maxPoints - (correctAnswerCount * pointsDecrement), minPoints);
                correctAnswerCount++;
            }
        }

        return state;
    }
}
