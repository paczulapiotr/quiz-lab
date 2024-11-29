using Microsoft.EntityFrameworkCore;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Messages.Game;
using Quiz.Master.Game;
using Quiz.Master.Persistance;

namespace Quiz.Master;

public class GameEngineHostedService(
    ILogger<GameEngineHostedService> logger,
    IDbContextFactory<QuizDbContext> dbContextFactory,
    IOneTimeConsumer<GameStarted> gameStartedConsumer,
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
                var gameStart = await gameStartedConsumer.ConsumeFirstAsync(cancellationToken: token);
                var gameId = Guid.Parse(gameStart.GameId);
                // Initialize game engine 
                using var ctx = dbContextFactory.CreateDbContext();

                var game = await ctx.Games
                    .Include(x => x.Players)
                    .FirstOrDefaultAsync(x => x.Id == gameId, token);

                await gameEngine.Run(gameId, game!.Players.ToList(), game.Rounds.ToList(), token);

                // Start game engine for stats from DB
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


