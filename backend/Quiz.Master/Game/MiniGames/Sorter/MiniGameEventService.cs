using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Handlers.Sorter;
using Quiz.Master.Persistance.Repositories.Abstract;

namespace Quiz.Master.Game.MiniGames.Sorter;

internal static class Actions
{
    public static string RoundStart = "Sorter.RoundStart";
    public static string RoundStarted = "Sorter.RoundStarted";
    public static string RoundEnd = "Sorter.RoundEnd";
    public static string RoundSummary = "Sorter.RoundSummary";
}

internal static class Interactions
{
    public static string SortSeletion = "Sorter.SortSeletion";
}

internal class MiniGameEventService(
    ILogger<MiniGameEventService> logger,
    IOneTimeConsumer<PlayerInteraction> playerInteraction,
    IOneTimeConsumer<MiniGameUpdate> miniGameUpdate,
    IGameRepository repository,
    IPublisher publisher
) : IMiniGameEventService
{
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    private bool _isSetup = false;
    private static string Type => MiniGameType.MusicGuess.ToString();

    public async Task Initialize(string gameId, CancellationToken cancellationToken = default)
    {
        if (_isSetup) return;

        try
        {
            await _semaphore.WaitAsync(cancellationToken);
            await playerInteraction.RegisterAsync(gameId, cancellationToken);
            await miniGameUpdate.RegisterAsync(gameId, cancellationToken);
            _isSetup = true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while setting up MiniGameEventService");
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }


    public async Task SendOnRoundStart(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.RoundStart), gameId, cancellationToken);
    }

    public async Task WaitForRoundStarted(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == Actions.RoundStarted, cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Actions.RoundStarted), gameId, cancellationToken);
    }

    public async Task SendOnRoundEnd(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.RoundStart), gameId, cancellationToken);
    }

    public async Task WaitForRoundSummary(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == Actions.RoundSummary, cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Actions.RoundSummary), gameId, cancellationToken);
    }

    public async Task<SortSelection?> GetSortSelection(string gameId, CancellationToken cancellationToken = default)
    {
        var selection = await playerInteraction.ConsumeFirstAsync(
            condition: x => x.GameId == gameId
                && x.InteractionType == Interactions.SortSeletion
                && x.Value != null,
            cancellationToken: cancellationToken);

        if (selection?.Value is null) return null;

        var players = await GetPlayersAsync(gameId, cancellationToken);
        var playerId = players.First(x => x.DeviceId == selection.DeviceId).Id;

        var split = selection.Value.Split("_");
        var itemId = split[0];
        var categoryId = split[1];

        return new SortSelection(playerId, itemId, categoryId, selection.Timestamp);
    }

    private async Task<IEnumerable<Player>> GetPlayersAsync(string gameId, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(gameId, out var id)) throw new ArgumentException("Invalid gameId");
        var game = await repository.FindAsync(id, cancellationToken);
        return game.Players;
    }
}