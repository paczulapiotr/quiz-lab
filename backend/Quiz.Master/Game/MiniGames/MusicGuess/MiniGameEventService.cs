using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Abstract;
using Quiz.Master.MiniGames.Handlers.MusicGuess;
using Quiz.Master.Persistance.Repositories.Abstract;

namespace Quiz.Master.Game.MiniGames.MusicGuess;

internal static class Actions
{
    public static string CategoryShowStart = "Music.CategoryShowStart";
    public static string CategoryShowStop = "Music.CategoryShowStop";
    public static string CategorySelectStart = "Music.CategorySelectStart";
    public static string QuestionAnswerShowStart = "Music.QuestionAnswerShowStart";
    public static string QuestionAnswerShowStop = "Music.QuestionAnswerShowStop";
    public static string QuestionAnswerStart = "Music.QuestionAnswerStart";
}

internal static class Interactions
{
    public static string CategorySelection = "Music.CategorySelection";
    public static string QuestionAnswer = "Music.QuestionAnswer";
}

internal class MiniGameEventService : MiniGameEventServiceBase, IMiniGameEventService
{
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
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    private readonly ILogger<MiniGameEventService> _logger;
    private readonly IOneTimeConsumer<PlayerInteraction> _playerInteraction;
    private readonly IGameRepository _repository;
    private bool _isSetup = false;
    protected override string Type => MiniGameType.MusicGuess.ToString();

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

    public async Task<IMiniGameEventService.CategorySelection?> GetCategorySelection(string gameId, CancellationToken cancellationToken = default)
    {
        var selection = await _playerInteraction.ConsumeFirstAsync(
            condition: x => x.GameId == gameId
                && x.InteractionType == Interactions.CategorySelection
                && x.Value != null,
            cancellationToken: cancellationToken);

        if (selection?.Value is null) return null;

        var players = await GetPlayersAsync(gameId, cancellationToken);
        var playerId = players.First(x => x.DeviceId == selection.DeviceId).Id;

        return new IMiniGameEventService.CategorySelection(playerId, selection.Value);
    }

    public async Task<IMiniGameEventService.AnswerSelection?> GetQuestionAnswerSelection(string gameId, CancellationToken cancellationToken = default)
    {
        var answer = await _playerInteraction.ConsumeFirstAsync(
            condition: x => x.GameId == gameId
                && x.InteractionType == Interactions.QuestionAnswer
                && x.Value != null,
        cancellationToken: cancellationToken);

        if (answer?.Value is null) return null;

        var players = await GetPlayersAsync(gameId, cancellationToken);
        var playerId = players.First(x => x.DeviceId == answer.DeviceId).Id;

        return new IMiniGameEventService.AnswerSelection(playerId, answer.Value, answer.Timestamp);
    }


    public Task SendOnCategorySelected(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.CategoryShowStart, cancellationToken);

    public  Task SendOnCategorySelection(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.CategorySelectStart, cancellationToken);

    public  Task SendOnQuestionAnswersPresentation(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.QuestionAnswerShowStart, cancellationToken);

    public  Task SendOnQuestionSelection(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.QuestionAnswerStart, cancellationToken);

    public  Task WaitForCategoryPresented(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => WaitFor(gameId, miniGameId, Actions.CategoryShowStop, cancellationToken);

    public  Task WaitForQuestionAnswersPresented(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => WaitFor(gameId, miniGameId, Actions.QuestionAnswerShowStop, cancellationToken);

    private async Task<IEnumerable<Player>> GetPlayersAsync(string gameId, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(gameId, out var id)) throw new ArgumentException("Invalid gameId");
        var game = await _repository.FindAsync(id, cancellationToken);
        return game.Players;
    }
}