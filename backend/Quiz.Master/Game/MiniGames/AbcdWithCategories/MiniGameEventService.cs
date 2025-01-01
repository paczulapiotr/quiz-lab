using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Handlers.AbcdWithCategories;
using Quiz.Master.MiniGames.Models.AbcdCategories;

namespace Quiz.Master.Game.MiniGames.AbcdWithCategories;

public class MiniGameEvenService(
    ILogger<MiniGameEvenService> logger,
    IOneTimeConsumer<PlayerInteraction> playerInteraction,
    IOneTimeConsumer<MiniGameUpdate> miniGameUpdate,
    IPublisher publisher) : IMiniGameEventService
{
    private static string Type => MiniGameType.AbcdWithCategories.ToString();

    public async Task SendOnPowerPlayExplain(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: "PowerPlayExplainStart"), cancellationToken);
    }

    public async Task WaitForPowerPlayExplained(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == "PowerPlayExplainStop", cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: "PowerPlayExplainStop"), cancellationToken);
    }

    public async Task<IMiniGameEventService.CategorySelection?> GetCategorySelection(string gameId, CancellationToken cancellationToken = default)
    {
        var selection = await playerInteraction.ConsumeFirstAsync(
            condition: x => x.GameId == gameId
                && x.InteractionType == "CategorySelection"
                && x.Value != null,
            cancellationToken: cancellationToken);

        if (selection?.Value is null) return null;

        return new IMiniGameEventService.CategorySelection(selection.DeviceId, selection.Value);

    }

    public async Task<IMiniGameEventService.PowerPlaySelection?> GetPowerPlaySelection(string gameId, CancellationToken cancellationToken = default)
    {
        var selection = await playerInteraction.ConsumeFirstAsync(
            condition: x => x.GameId == gameId
                && x.InteractionType == "PowerPlaySelection",
            cancellationToken: cancellationToken);

        try
        {
            var actionData = selection!.Data!;
            var deviceId = actionData["deviceId"]!;
            var powerPlay = Enum.Parse<PowerPlay>(actionData["powerPlay"]);
            return new IMiniGameEventService.PowerPlaySelection(deviceId, powerPlay);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while parsing PowerPlay selection");
            return null;
        }
    }

    public async Task SendOnPowerPlayApplication(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: "PowerPlayApplyStart"), cancellationToken);
    }

    public async Task<IMiniGameEventService.AnswerSelection?> GetQuestionAnswerSelection(string gameId, CancellationToken cancellationToken = default)
    {
        var answer = await playerInteraction.ConsumeFirstAsync(
            condition: x => x.GameId == gameId
                && x.InteractionType == "QuestionAnswer"
                && x.Value != null,
        cancellationToken: cancellationToken);

        if (answer?.Value is null) return null;

        return new IMiniGameEventService.AnswerSelection(answer.DeviceId, answer.Value, answer.Timestamp);
    }

    public async Task SendOnCategorySelected(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: "CategoryShowStart"), cancellationToken);
    }

    public async Task SendOnCategorySelection(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: "CategorySelectStart"), cancellationToken);
    }

    public async Task SendOnPowerPlayStart(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: "PowerPlayStart"), cancellationToken);
    }

    public async Task SendOnQuestionAnswersPresentation(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: "QuestionAnswerShowStart"), cancellationToken);
    }

    public async Task SendOnQuestionPresentation(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: "QuestionShowStart"), cancellationToken);
    }

    public async Task SendOnQuestionSelection(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: "QuestionAnswerStart"), cancellationToken);
    }

    public async Task WaitForCategoryPresented(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == "CategoryShowStop", cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, "CategoryShowStop"), cancellationToken);
    }

    public async Task WaitForPowerPlayApplied(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == "PowerPlayApplyStop", cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: "PowerPlayApplyStop"), cancellationToken);
    }

    public async Task WaitForQuestionAnswersPresented(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == "QuestionAnswerShowStop", cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, "QuestionAnswerShowStop"), cancellationToken);
    }

    public async Task WaitForQuestionPresented(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == "QuestionShowStop", cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, "QuestionShowStop"), cancellationToken);
    }
}
