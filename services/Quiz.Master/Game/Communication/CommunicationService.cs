using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;

namespace Quiz.Master.Game.Communication;

public class CommunicationService(
    IPublisher publisher,
    IOneTimeConsumer<GameStatusUpdate> gameStatusConsumer)
     : ICommunicationService
{

    // public async Task SendRoundEndingMessage(string gameId, CancellationToken cancellationToken = default)
    // {
    //     await publisher.PublishAsync(new GameStatusUpdate(gameId, GameStatus.RoundEnding), cancellationToken);
    //     Task.Delay(10_000)
    //         .ContinueWith(async (cancel) =>
    //         {
    //             await publisher.PublishAsync(new GameStatusUpdate(gameId, GameStatus.RoundEnded), cancellationToken);
    //         })
    //         .ConfigureAwait(false);
    // }

    // public async Task ReceiveRoundEndedMessage(string gameId, CancellationToken cancellationToken = default)
    // {
    //     await ReceiveStatusMessage(gameId, GameStatus.RoundEnded, cancellationToken: cancellationToken);
    // }

    // public async Task SendRoundStartingMessage(string gameId, CancellationToken cancellationToken = default)
    // {
    //     await publisher.PublishAsync(new GameStatusUpdate(gameId, GameStatus.RoundStarting), cancellationToken);
    //     Task.Delay(10_000)
    //         .ContinueWith(async (cancel)
    //             =>
    //         {
    //             await publisher.PublishAsync(new GameStatusUpdate(gameId, GameStatus.RoundStarted), cancellationToken);
    //         })
    //                 .ConfigureAwait(false);
    // }

    // public async Task ReceiveRoundStartedMessage(string gameId, CancellationToken cancellationToken = default)
    // {
    //     await ReceiveStatusMessage(gameId, GameStatus.RoundStarted, cancellationToken: cancellationToken);
    // }

    public async Task SendRulesExplainMessage(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new GameStatusUpdate(gameId, GameStatus.RulesExplaining), cancellationToken);
    }

    public async Task ReceiveRulesExplainedMessage(string gameId, CancellationToken cancellationToken = default)
    {
        await ReceiveStatusMessage(gameId, GameStatus.RulesExplained, cancellationToken: cancellationToken);
    }

    public async Task SendGameEndingMessage(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new GameStatusUpdate(gameId, GameStatus.GameEnding), cancellationToken);
    }

    public async Task ReceiveGameEndedMessage(string gameId, CancellationToken cancellationToken = default)
    {
        await ReceiveStatusMessage(gameId, GameStatus.GameEnded, cancellationToken: cancellationToken);
    }

    public async Task SendMiniGameStartingMessage(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new GameStatusUpdate(gameId, GameStatus.MiniGameStarting), cancellationToken);
    }

    public async Task ReceiveMiniGameStartedMessage(string gameId, CancellationToken cancellationToken = default)
    {
        await ReceiveStatusMessage(gameId, GameStatus.MiniGameStarted, cancellationToken: cancellationToken);
    }

    public async Task SendMiniGameEndingMessage(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new GameStatusUpdate(gameId, GameStatus.MiniGameEnding), cancellationToken);
    }

    public async Task ReceiveMiniGameEndedMessage(string gameId, CancellationToken cancellationToken = default)
    {
        await ReceiveStatusMessage(gameId, GameStatus.MiniGameEnded, cancellationToken: cancellationToken);
    }

    private async Task ReceiveStatusMessage(string gameId, GameStatus status, CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var gameStatus = await gameStatusConsumer.ConsumeFirstAsync(cancellationToken: cancellationToken);
            if (gameStatus.GameId == gameId && gameStatus.Status == status)
            {
                return;
            }
        }
    }
}