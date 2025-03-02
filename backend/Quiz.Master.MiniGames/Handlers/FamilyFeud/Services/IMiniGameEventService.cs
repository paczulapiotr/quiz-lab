namespace Quiz.Master.MiniGames.Handlers.FamilyFeud;

public interface IMiniGameEventService
{
    public Task Initialize(string gameId, CancellationToken cancellationToken = default);

    public Task SendOnAnswerStart(string gameId, string miniGameId, CancellationToken cancellationToken = default);
    public Task WaitOnAsnwered(string gameId, string miniGameId, CancellationToken cancellationToken = default);

    public Task SendOnQuestionShow(string gameId, string miniGameId, CancellationToken cancellationToken = default);
    public Task WaitForQuestionShown(string gameId, string miniGameId, CancellationToken cancellationToken = default);

    public Task SendOnAnswerShow(string gameId, string miniGameId, CancellationToken cancellationToken = default);
    public Task WaitForAnswerShown(string gameId, string miniGameId, CancellationToken cancellationToken = default);

    public Task SendOnRoundEnd(string gameId, string miniGameId, CancellationToken cancellationToken = default);
    public Task WaitForRoundEnded(string gameId, string miniGameId, CancellationToken cancellationToken = default);
    Task<AnswerSelection?> GetAnswer(string gameId, CancellationToken cancellationToken = default);

    public record AnswerSelection(Guid PlayerId, string Answer, DateTime Timestamp);
}