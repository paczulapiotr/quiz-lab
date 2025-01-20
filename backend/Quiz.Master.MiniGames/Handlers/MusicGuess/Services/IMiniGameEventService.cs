namespace Quiz.Master.MiniGames.Handlers.MusicGuess;

public interface IMiniGameEventService
{
    public Task Initialize(string gameId, CancellationToken cancellationToken = default);
    public Task SendOnCategorySelection(string gameId, CancellationToken cancellationToken = default);
    public Task<CategorySelection?> GetCategorySelection(string gameId, CancellationToken cancellationToken = default);
    public Task SendOnCategorySelected(string gameId, CancellationToken cancellationToken = default);
    public Task WaitForCategoryPresented(string gameId, CancellationToken cancellationToken = default);
    public Task SendOnQuestionSelection(string gameId, CancellationToken cancellationToken = default);
    public Task<AnswerSelection?> GetQuestionAnswerSelection(string gameId, CancellationToken cancellationToken = default);
    public Task SendOnQuestionAnswersPresentation(string gameId, CancellationToken cancellationToken = default);
    public Task WaitForQuestionAnswersPresented(string gameId, CancellationToken cancellationToken = default);

    public record CategorySelection(Guid PlayerId, string CategoryId);

    public record AnswerSelection(Guid PlayerId, string AnswerId, DateTime Timestamp);
}