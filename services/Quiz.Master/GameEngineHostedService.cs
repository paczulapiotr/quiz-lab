using Quiz.Common.Broker.Consumer;
using Quiz.Common.Messages.Game;
using Quiz.Master.Game;

namespace Quiz.Master;

public class GameEngineHostedService(
    ILogger<GameEngineHostedService> logger,
    IOneTimeConsumer<GameStatusUpdate> gameStatusConsumer,
    IServiceScopeFactory serviceScopeFactory) : IHostedService
{

    private CancellationTokenSource? cancellationTokenSource;
    private Task? backgroundTask;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("GameEngineHostedService is starting.");
        using var scope = serviceScopeFactory.CreateScope();
        var gameEngine = scope.ServiceProvider.GetRequiredService<IGameEngine>();

        cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var token = cancellationTokenSource.Token;

        backgroundTask = Task.Run(async () =>
        {
            while (!token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
                // Listen for game start rabbitmq message
                var status = await gameStatusConsumer.ConsumeFirstAsync(cancellationToken: token);
                if (status?.Status != GameStatus.GameStarted)
                {
                    continue;
                }

                var gameId = Guid.Parse(status.GameId);

                // Initialize game engine 
                await gameEngine.Run(gameId, token);
            }
        }, token);

        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("GameEngineHostedService is stopping.");

        if (backgroundTask != null)
        {
            cancellationTokenSource?.Cancel();

            try
            {
                await backgroundTask;
            }
            catch (OperationCanceledException)
            {
                // Ignore the cancellation exception
            }
        }
    }
}


