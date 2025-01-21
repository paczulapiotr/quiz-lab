using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Handlers.AbcdWithCategories;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Master.Persistance.Repositories.Abstract;

namespace Quiz.Master.Game.MiniGames.AbcdWithCategories;

public static class Actions {
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

public static class Interactions {
    public static string CategorySelection = "Abcd.CategorySelection";
    public static string PowerPlaySelection = "Abcd.PowerPlaySelection";
    public static string QuestionAnswer = "Abcd.QuestionAnswer";
}

public class MiniGameEventService(
    ILogger<MiniGameEventService> logger,
    IOneTimeConsumer<PlayerInteraction> playerInteraction,
    IOneTimeConsumer<MiniGameUpdate> miniGameUpdate,
    IGameRepository repository,
    IPublisher publisher) : IMiniGameEventService
{
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    private string? _setupId;
    private static string Type => MiniGameType.AbcdWithCategories.ToString();

    public async Task Initialize(string gameId, CancellationToken cancellationToken)
    {
        if(_setupId == gameId) return;

        try
        {
            await _semaphore.WaitAsync(cancellationToken);
            await playerInteraction.RegisterAsync(gameId, cancellationToken);
            await miniGameUpdate.RegisterAsync(gameId, cancellationToken);
            _setupId = gameId;
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

    public async Task SendOnPowerPlayExplain(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.PowerPlayExplainStart), gameId, cancellationToken);
    }

    public async Task WaitForPowerPlayExplained(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == Actions.PowerPlayExplainStop, cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.PowerPlayExplainStop), gameId, cancellationToken);
    }

    public async Task<IMiniGameEventService.CategorySelection?> GetCategorySelection(string gameId, CancellationToken cancellationToken = default)
    {
 
        var selection = await playerInteraction.ConsumeFirstAsync(
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
        var selection = await playerInteraction.ConsumeFirstAsync(
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
            logger.LogError(e, "Error while parsing PowerPlay selection");
            return null;
        }
    }

    public async Task SendOnPowerPlayApplication(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.PowerPlayApplyStart), gameId, cancellationToken);
    }

    public async Task<IMiniGameEventService.AnswerSelection?> GetQuestionAnswerSelection(string gameId, CancellationToken cancellationToken = default)
    {
        var answer = await playerInteraction.ConsumeFirstAsync(
            condition: x => x.GameId == gameId
                && x.InteractionType == Interactions.QuestionAnswer
                && x.Value != null,
        cancellationToken: cancellationToken);

        if (answer?.Value is null) return null;

        var players = await GetPlayersAsync(gameId, cancellationToken);
        var playerId = players.First(x => x.DeviceId == answer.DeviceId).Id;

        return new IMiniGameEventService.AnswerSelection(playerId, answer.Value, answer.Timestamp);
    }

    public async Task SendOnCategorySelected(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.CategoryShowStart), gameId, cancellationToken);
    }

    public async Task SendOnCategorySelection(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.CategorySelectStart), gameId, cancellationToken);
    }

    public async Task SendOnPowerPlayStart(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.PowerPlayStart), gameId, cancellationToken);
    }

    public async Task SendOnQuestionAnswersPresentation(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.QuestionAnswerShowStart), gameId, cancellationToken);
    }

    public async Task SendOnQuestionPresentation(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.QuestionShowStart), gameId, cancellationToken);
    }

    public async Task SendOnQuestionSelection(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.QuestionAnswerStart), gameId, cancellationToken);
    }

    public async Task WaitForCategoryPresented(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == Actions.CategoryShowStop, cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Actions.CategoryShowStop), gameId, cancellationToken);
    }

    public async Task WaitForPowerPlayApplied(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == Actions.PowerPlayApplyStop, cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.PowerPlayApplyStop), gameId, cancellationToken);
    }

    public async Task WaitForQuestionAnswersPresented(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == Actions.QuestionAnswerShowStop, cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Actions.QuestionAnswerShowStop), gameId, cancellationToken);
    }

    public async Task WaitForQuestionPresented(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == Actions.QuestionShowStop, cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Actions.QuestionShowStop), gameId, cancellationToken);
    }

    private async Task<IEnumerable<Player>> GetPlayersAsync(string gameId, CancellationToken cancellationToken = default)
    {
        if(!Guid.TryParse(gameId, out var id)) throw new ArgumentException("Invalid gameId");
        var game = await repository.FindAsync(id, cancellationToken);
        return game.Players;
    }
}
