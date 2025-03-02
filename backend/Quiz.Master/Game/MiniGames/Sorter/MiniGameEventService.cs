using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Abstract;
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
    public static string SortSeletion = "Sorter.SortSelection";
}

internal class MiniGameEventService : MiniGameEventServiceBase, IMiniGameEventService
{
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    private readonly ILogger<MiniGameEventService> _logger;
    private readonly IOneTimeConsumer<PlayerInteraction> _playerInteraction;
    private readonly IGameRepository _repository;
    private bool _isSetup = false;
    protected override string Type => MiniGameType.MusicGuess.ToString();

    public MiniGameEventService(ILogger<MiniGameEventService> logger,
    IOneTimeConsumer<PlayerInteraction> playerInteraction,
    IOneTimeConsumer<MiniGameUpdate> miniGameUpdate,
    IMiniGameRepository miniGameRepository,
    IGameRepository repository,
    IPublisher publisher) : base(publisher, miniGameUpdate, miniGameRepository)
    {
        _logger = logger;
        _playerInteraction = playerInteraction;
        _repository = repository;
    }

    public override async Task Initialize(string gameId, CancellationToken cancellationToken = default)
    {
        if (_isSetup) return;

        try
        {
            await _semaphore.WaitAsync(cancellationToken);
            await _playerInteraction.RegisterAsync(gameId, cancellationToken);
            await base.Initialize(gameId, cancellationToken);
            _isSetup = true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while setting up MiniGameEventService");
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }


    public Task SendOnRoundStart(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.RoundStart, cancellationToken);

    public Task WaitForRoundStarted(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => WaitFor(gameId, miniGameId, Actions.RoundStarted, cancellationToken);

    public Task SendOnRoundEnd(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.RoundEnd, cancellationToken);

    public Task WaitForRoundSummary(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => WaitFor(gameId, miniGameId, Actions.RoundSummary, cancellationToken);

    public async Task<SortSelection?> GetSortSelection(string gameId, CancellationToken cancellationToken = default)
    {
        var selection = await _playerInteraction.ConsumeFirstAsync(
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
        var game = await _repository.FindAsync(id, cancellationToken);
        return game.Players;
    }

    public Task Initialize(string gameId, string miniGameId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}