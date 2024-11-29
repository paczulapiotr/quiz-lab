namespace Quiz.Master.Game.Communication;

public interface ICommunicationService
{
    Task SendRulesExplainMessage(string gameId, CancellationToken cancellationToken = default);
    Task ReceiveRulesExplainedMessage(CancellationToken cancellationToken = default);
    Task SendRoundStartMessage(string gameId, CancellationToken cancellationToken = default);
    Task ReceiveRoundStartedMessage(CancellationToken cancellationToken = default);
    Task SendRoundEndMessage(string gameId, CancellationToken cancellationToken = default);
    Task ReceiveRoundEndedMessage(CancellationToken cancellationToken = default);
    Task SendGameEndMessage(string gameId, CancellationToken cancellationToken = default);
}
