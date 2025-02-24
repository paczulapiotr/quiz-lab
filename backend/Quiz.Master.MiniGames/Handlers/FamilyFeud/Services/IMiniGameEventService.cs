namespace Quiz.Master.MiniGames.Handlers.FamilyFeud;

public interface IMiniGameEventService
{
    public Task Initialize(string gameId, CancellationToken cancellationToken = default);

    public Task SendOnAnswerStart(string gameId, CancellationToken cancellationToken = default);
    public Task WaitOnAsnwered(string gameId, CancellationToken cancellationToken = default);

    public Task SendOnQuestionShow(string gameId, CancellationToken cancellationToken = default);
    public Task WaitForQuestionShown(string gameId, CancellationToken cancellationToken = default);

    public Task SendOnAnswerShow(string gameId, CancellationToken cancellationToken = default);
    public Task WaitForAnswerShown(string gameId, CancellationToken cancellationToken = default);

    public Task SendOnRoundEnd(string gameId, CancellationToken cancellationToken = default);
    public Task WaitForRoundEnded(string gameId, CancellationToken cancellationToken = default);
    Task<AnswerSelection?> GetAnswer(string gameId, CancellationToken cancellationToken = default);

    public record AnswerSelection(Guid PlayerId, string Answer, DateTime Timestamp);
}