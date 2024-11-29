using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;
using Quiz.Common.Messages.Round;

namespace Quiz.Master.Game.Communication;

public class CommunicationService(
    IPublisher publisher,
    IOneTimeConsumer<RulesExplained> rulesExplainedConsumer,
    IOneTimeConsumer<RoundStarted> roundStartedConsumer,
    IOneTimeConsumer<RoundEnded> roundEndedConsumer)
     : ICommunicationService
{
    public async Task SendGameEndMessage(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new GameEnd(gameId), cancellationToken);
    }

    public async Task SendRoundEndMessage(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new RoundEnd(gameId), cancellationToken);
    }

    public async Task ReceiveRoundEndedMessage(CancellationToken cancellationToken = default)
    {
        await roundEndedConsumer.ConsumeFirstAsync(cancellationToken: cancellationToken);
    }

    public async Task SendRoundStartMessage(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new RoundStart(gameId), cancellationToken);
    }

    public async Task ReceiveRoundStartedMessage(CancellationToken cancellationToken = default)
    {
        await roundStartedConsumer.ConsumeFirstAsync(cancellationToken: cancellationToken);
    }

    public async Task SendRulesExplainMessage(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new RulesExplain(gameId), cancellationToken);
    }

    public async Task ReceiveRulesExplainedMessage(CancellationToken cancellationToken = default)
    {
        await rulesExplainedConsumer.ConsumeFirstAsync(cancellationToken: cancellationToken);
    }
}