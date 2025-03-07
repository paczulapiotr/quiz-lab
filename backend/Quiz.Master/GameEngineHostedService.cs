using Quiz.Common.Broker.Consumer;
using Quiz.Common.Messages.Game;
using Quiz.Master.Game;

namespace Quiz.Master;

public class GameEngineHostedService(
    ILogger<GameEngineHostedService> logger,
    IOneTimeConsumer<NewGameCreation> newGameConsumer,
    IServiceScopeFactory serviceScopeFactory) : IHostedService
{

    private CancellationTokenSource? _mainCancellationTokenSource = null!;
    private Task? backgroundTask;
    private List<(Task task, CancellationTokenSource tokenSource)> instanceTasks = new();

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _mainCancellationTokenSource = new CancellationTokenSource();
        var token = _mainCancellationTokenSource.Token;

        logger.LogInformation("GameEngineHostedService is starting.");
        backgroundTask = Task.Run(async () =>
        {
            await newGameConsumer.RegisterAsync(cancellationToken: cancellationToken);

            var informer = Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, cancellationToken);
                    if (!newGameConsumer.IsConnected)
                    {
                        logger.LogInformation("GameEngineHostedService - status - disconnected");
                    }
                }
            });

            while (!token.IsCancellationRequested)
            {
                try
                {
                    // Listen for game start rabbitmq message
                    var message = await newGameConsumer.ConsumeFirstAsync();
                    if (message == null)
                    {
                        continue;
                    }

                    var gameId = Guid.Parse(message.GameId);
                    var gameToken = _mainCancellationTokenSource.Token;

                    // Initialize game engine 
                    using var scope = serviceScopeFactory.CreateScope();
                    var gameEngine = scope.ServiceProvider.GetRequiredService<IGameEngine>();
                    var (t, source) = CreateToken(gameToken);
                    var gameTask = gameEngine.Run(gameId, t);

                    ManageGameTask(gameTask, source);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error in GameEngineHostedService");
                }
            }

            logger.LogInformation("GameEngineHostedService background task is stopping.");
        }, token);

        await Task.CompletedTask;
    }

    private void ManageGameTask(Task gameTask, CancellationTokenSource source)
    {
        instanceTasks.RemoveAll(x => x.task.IsCanceled || x.task.IsCompleted || x.task.IsFaulted);
        instanceTasks.Add((gameTask, source));
        logger.LogInformation("GameEngineHostedService - currently running {0} games", instanceTasks.Count);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("GameEngineHostedService is stopping.");

        if (backgroundTask != null)
        {
            _mainCancellationTokenSource?.Cancel();

            try
            {
                await Task.WhenAll([backgroundTask, .. instanceTasks.Select(x => x.task)]);
            }
            catch (OperationCanceledException)
            {
                // Ignore the cancellation exception
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error stopping GameEngineHostedService");
            }
        }
    }

    private (CancellationToken token, CancellationTokenSource source) CreateToken(CancellationToken cancellationToken)
    {
        var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cancellationTokenSource.CancelAfter(TimeSpan.FromHours(2));
        var token = cancellationTokenSource.Token;

        return (token, cancellationTokenSource);
    }
}


