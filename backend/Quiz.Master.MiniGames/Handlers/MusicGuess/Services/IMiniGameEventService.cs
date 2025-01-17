using Quiz.Master.MiniGames.Models.AbcdCategories;

namespace Quiz.Master.MiniGames.Handlers.MusicGuess;

public interface IMiniGameEventService
{
    public Task Initialize(string gameId, CancellationToken cancellationToken = default);
    public record AnswerSelection(Guid PlayerId, string AnswerId, DateTime Timestamp);
}