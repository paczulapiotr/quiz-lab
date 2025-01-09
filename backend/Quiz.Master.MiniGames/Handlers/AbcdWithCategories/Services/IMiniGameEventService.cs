using Quiz.Master.MiniGames.Models.AbcdCategories;

namespace Quiz.Master.MiniGames.Handlers.AbcdWithCategories;

public interface IMiniGameEventService
{
    public Task Initialize(string gameId, CancellationToken cancellationToken = default);
    public Task SendOnPowerPlayExplain(string gameId, CancellationToken cancellationToken = default);
    public Task WaitForPowerPlayExplained(string gameId, CancellationToken cancellationToken = default);
    public Task SendOnPowerPlayStart(string gameId, CancellationToken cancellationToken = default);
    public Task<PowerPlaySelection?> GetPowerPlaySelection(string gameId, CancellationToken cancellationToken = default);
    public Task SendOnPowerPlayApplication(string gameId, CancellationToken cancellationToken = default);
    public Task WaitForPowerPlayApplied(string gameId, CancellationToken cancellationToken = default);
    public Task SendOnCategorySelection(string gameId, CancellationToken cancellationToken = default);
    public Task<CategorySelection?> GetCategorySelection(string gameId, CancellationToken cancellationToken = default);
    public Task SendOnCategorySelected(string gameId, CancellationToken cancellationToken = default);
    public Task WaitForCategoryPresented(string gameId, CancellationToken cancellationToken = default);
    public Task SendOnQuestionPresentation(string gameId, CancellationToken cancellationToken = default);
    public Task WaitForQuestionPresented(string gameId, CancellationToken cancellationToken = default);
    public Task SendOnQuestionSelection(string gameId, CancellationToken cancellationToken = default);
    public Task<AnswerSelection?> GetQuestionAnswerSelection(string gameId, CancellationToken cancellationToken = default);
    public Task SendOnQuestionAnswersPresentation(string gameId, CancellationToken cancellationToken = default);
    public Task WaitForQuestionAnswersPresented(string gameId, CancellationToken cancellationToken = default);


    public record PowerPlaySelection(Guid PlayerId, Guid TargetPlayerId, PowerPlay PowerPlay);

    public record CategorySelection(Guid PlayerId, string CategoryId);

    public record AnswerSelection(Guid PlayerId, string AnswerId, DateTime Timestamp);
}