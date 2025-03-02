using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Abstract;
using Quiz.Master.MiniGames.Handlers.AbcdWithCategories;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Master.Persistance.Repositories.Abstract;

namespace Quiz.Master.Game.MiniGames.AbcdWithCategories;

public static class Actions
{
    public static string PowerPlayExplainStart = "Abcd.PowerPlayExplainStart";
    public static string PowerPlayExplainStop = "Abcd.PowerPlayExplainStop";
    public static string PowerPlayApplyStart = "Abcd.PowerPlayApplyStart";
    public static string PowerPlayApplyStop = "Abcd.PowerPlayApplyStop";
    public static string CategoryShowStart = "Abcd.CategoryShowStart";
    public static string CategoryShowStop = "Abcd.CategoryShowStop";
    public static string CategorySelectStart = "Abcd.CategorySelectStart";
    public static string PowerPlayStart = "Abcd.PowerPlayStart";
    public static string QuestionAnswerShowStart = "Abcd.QuestionAnswerShowStart";
    public static string QuestionAnswerShowStop = "Abcd.QuestionAnswerShowStop";
    public static string QuestionShowStart = "Abcd.QuestionShowStart";
    public static string QuestionShowStop = "Abcd.QuestionShowStop";
    public static string QuestionAnswerStart = "Abcd.QuestionAnswerStart";
}

public static class Interactions
{
    public static string CategorySelection = "Abcd.CategorySelection";
    public static string PowerPlaySelection = "Abcd.PowerPlaySelection";
    public static string QuestionAnswer = "Abcd.QuestionAnswer";
}

public class MiniGameEventService : MiniGameEventServiceBase, IMiniGameEventService
{

    public MiniGameEventService(
    ILogger<MiniGameEventService> logger,
    IOneTimeConsumer<PlayerInteraction> playerInteraction,
    IOneTimeConsumer<MiniGameUpdate> miniGameUpdate,
    IGameRepository repository,
    IMiniGameRepository miniGameRepository,
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
    private string? _setupId;
    override protected string Type => MiniGameType.AbcdWithCategories.ToString();

    public override async Task Initialize(string gameId, CancellationToken cancellationToken)
    {
        if (_setupId == gameId) return;

        try
        {
            await _semaphore.WaitAsync(cancellationToken);
            await _playerInteraction.RegisterAsync(gameId, cancellationToken);
            await base.Initialize(gameId, cancellationToken);
            _setupId = gameId;
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


    public Task SendOnPowerPlayExplain(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.PowerPlayExplainStart, cancellationToken);

    public Task WaitForPowerPlayExplained(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => WaitFor(gameId, miniGameId, Actions.PowerPlayExplainStop, cancellationToken);

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

    public async Task<IMiniGameEventService.PowerPlaySelection?> GetPowerPlaySelection(string gameId, CancellationToken cancellationToken = default)
    {
        var selection = await _playerInteraction.ConsumeFirstAsync(
            condition: x => x.GameId == gameId
                && x.InteractionType == Interactions.PowerPlaySelection,
            cancellationToken: cancellationToken);

        try
        {
            var actionData = selection!.Data!;
            var targetDeviceId = actionData["deviceId"]!;
            var powerPlay = Enum.Parse<PowerPlay>(actionData["powerPlay"]);

            var players = await GetPlayersAsync(gameId, cancellationToken);
            var playerId = players.First(x => x.DeviceId == selection.DeviceId).Id;
            var targetPlayerId = players.First(x => x.DeviceId == targetDeviceId).Id;

            return new IMiniGameEventService.PowerPlaySelection(playerId, targetPlayerId, powerPlay);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while parsing PowerPlay selection");
            return null;
        }
    }

    public Task SendOnPowerPlayApplication(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.PowerPlayApplyStart, cancellationToken);

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


    public Task SendOnCategorySelection(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.CategorySelectStart, cancellationToken);

    public Task SendOnPowerPlayStart(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.PowerPlayStart, cancellationToken);

    public Task SendOnQuestionAnswersPresentation(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.QuestionAnswerShowStart, cancellationToken);

    public  Task SendOnQuestionPresentation(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.QuestionShowStart, cancellationToken);

    public Task SendOnQuestionSelection(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.QuestionAnswerStart, cancellationToken);

    public Task WaitForCategoryPresented(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => WaitFor(gameId, miniGameId, Actions.CategoryShowStop, cancellationToken);

    public Task WaitForPowerPlayApplied(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => WaitFor(gameId, miniGameId, Actions.PowerPlayApplyStop, cancellationToken);

    public Task WaitForQuestionAnswersPresented(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => WaitFor(gameId, miniGameId, Actions.QuestionAnswerShowStop, cancellationToken);

    public Task WaitForQuestionPresented(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => WaitFor(gameId, miniGameId, Actions.QuestionShowStop, cancellationToken);

    private async Task<IEnumerable<Player>> GetPlayersAsync(string gameId, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(gameId, out var id)) throw new ArgumentException("Invalid gameId");
        var game = await _repository.FindAsync(id, cancellationToken);
        return game.Players;
    }
}
