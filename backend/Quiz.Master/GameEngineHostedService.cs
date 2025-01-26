using Quiz.Common.Broker.Consumer;
using Quiz.Common.Messages.Game;
using Quiz.Master.Game;

namespace Quiz.Master;

public class GameEngineHostedService(
    ILogger<GameEngineHostedService> logger,
    IOneTimeConsumer<NewGameCreation> newGameConsumer,
    IServiceScopeFactory serviceScopeFactory) : IHostedService
{

    private CancellationTokenSource? cancellationTokenSource;
    private Task? backgroundTask;
    private List<Task> instanceTasks = new();

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("GameEngineHostedService is starting.");
        cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var token = cancellationTokenSource.Token;

        backgroundTask = Task.Run(async () =>
        {
            await newGameConsumer.RegisterAsync(cancellationToken: token);

            var informer = Task.Run(async () => {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(1000, token);
                    if(!newGameConsumer.IsConnected)
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
                    if(message == null)
                    {
                        continue;
                    }
                    
                    var gameId = Guid.Parse(message.GameId);

                    // Initialize game engine 
                    using var scope = serviceScopeFactory.CreateScope();
                    var gameEngine = scope.ServiceProvider.GetRequiredService<IGameEngine>();
                    instanceTasks.Add(gameEngine.Run(gameId, CreateGameCancellationToken(token)));
                    instanceTasks.RemoveAll(x=>x.IsCompleted);
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

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("GameEngineHostedService is stopping.");

        if (backgroundTask != null)
        {
            cancellationTokenSource?.Cancel();

            try
            {
                await Task.WhenAll(new[] { backgroundTask }.Concat(instanceTasks).ToArray());
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

    private CancellationToken CreateGameCancellationToken(CancellationToken token) {
        var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
        tokenSource.CancelAfter(TimeSpan.FromHours(2));

        return tokenSource.Token;
    }
}


