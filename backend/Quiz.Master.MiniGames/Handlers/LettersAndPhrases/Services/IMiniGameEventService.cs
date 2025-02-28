namespace Quiz.Master.MiniGames.Handlers.LettersAndPhrases;

public interface IMiniGameEventService
{
    public Task Initialize(string gameId, CancellationToken cancellationToken = default);
    public Task SendOnQuestionShow(string gameId, string miniGameId, CancellationToken cancellationToken = default);
    public Task WaitForQuestionShown(string gameId, string miniGameId, CancellationToken cancellationToken = default);
    public Task SendOnAnswerStart(string gameId, string miniGameId, CancellationToken cancellationToken = default);
    public Task<AnswerSelection?> GetLetterSelection(string gameId, CancellationToken cancellationToken = default);
    public Task SendOnAnswered(string gameId, string miniGameId, CancellationToken cancellationToken = default);
    public Task SendOnPhraseSolvedPresentation(string gameId, string miniGameId, CancellationToken cancellationToken = default);
    public Task WaitForPhraseSolvedPresented(string gameId, string miniGameId, CancellationToken cancellationToken = default);

    public record AnswerSelection(Guid PlayerId, char Letter, DateTime Timestamp);
}