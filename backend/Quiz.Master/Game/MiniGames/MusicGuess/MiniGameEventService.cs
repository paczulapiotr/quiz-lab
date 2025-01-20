using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;
using Quiz.Master.Core.Models;
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

    public async Task SendOnQuestionAnswersPresentation(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.QuestionAnswerShowStart), gameId, cancellationToken);
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

    public async Task WaitForQuestionAnswersPresented(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == Actions.QuestionAnswerShowStop, cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Actions.QuestionAnswerShowStop), gameId, cancellationToken);
    }

    private async Task<IEnumerable<Player>> GetPlayersAsync(string gameId, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(gameId, out var id)) throw new ArgumentException("Invalid gameId");
        var game = await repository.FindAsync(id, cancellationToken);
        return game.Players;
    }
}