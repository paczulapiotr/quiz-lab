namespace Quiz.Master.Game.Communication;

public interface ICommunicationService
{
    Task Initialize(string gameId, CancellationToken cancellationToken = default);
    Task SendGameCreatedMessage(string gameId, CancellationToken cancellationToken = default);
    Task ReceiveGameStartedMessage(string gameId, CancellationToken cancellationToken = default);
    Task SendRulesExplainMessage(string gameId, CancellationToken cancellationToken = default);
    Task ReceiveRulesExplainedMessage(string gameId, CancellationToken cancellationToken = default);
    Task SendMiniGameStartingMessage(string gameId, CancellationToken cancellationToken = default);
    Task ReceiveMiniGameStartedMessage(string gameId, CancellationToken cancellationToken = default);
    Task SendMiniGameEndingMessage(string gameId, CancellationToken cancellationToken = default);
    Task ReceiveMiniGameEndedMessage(string gameId, CancellationToken cancellationToken = default);
    Task SendGameEndingMessage(string gameId, CancellationToken cancellationToken = default);
    Task ReceiveGameEndedMessage(string gameId, CancellationToken cancellationToken = default);
}
