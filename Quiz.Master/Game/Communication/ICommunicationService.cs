namespace Quiz.Master.Game.Communication;

public interface ICommunicationService
{
    Task SendRulesExplainMessage(string gameId, CancellationToken cancellationToken = default);
    Task ReceiveRulesExplainedMessage(string gameId, CancellationToken cancellationToken = default);
    Task SendRoundStartingMessage(string gameId, CancellationToken cancellationToken = default);
    Task ReceiveRoundStartedMessage(string gameId, CancellationToken cancellationToken = default);
    Task SendRoundEndingMessage(string gameId, CancellationToken cancellationToken = default);
    Task ReceiveRoundEndedMessage(string gameId, CancellationToken cancellationToken = default);
    Task SendGameEndingMessage(string gameId, CancellationToken cancellationToken = default);
    Task ReceiveGameEndedMessage(string gameId, CancellationToken cancellationToken = default);
}
