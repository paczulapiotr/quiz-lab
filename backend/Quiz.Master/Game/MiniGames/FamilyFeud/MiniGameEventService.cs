using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Handlers.FamilyFeud;
using Quiz.Master.Persistance.Repositories.Abstract;

namespace Quiz.Master.Game.MiniGames.FamilyFeud;

internal static class Actions
{
    public static string QuestionShown = "FamilyFeud.QuestionShown";
    public static string QuestionShow = "FamilyFeud.QuestionShow";
    public static string AnswerStart = "FamilyFeud.AnswerStart";
    public static string Answered = "FamilyFeud.Answered";
    public static string AnswerShow = "FamilyFeud.AnswerShow";
    public static string AnswerShown = "FamilyFeud.AnswerShown";
    public static string RoundEnd = "FamilyFeud.RoundEnd";
    public static string RoundEnded = "FamilyFeud.RoundEnded";
}

internal static class Interactions
{
    public static string Answer = "FamilyFeud.Answer";
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

    public async Task SendOnAnswerShow(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.AnswerShow), gameId, cancellationToken);
    }

    public async Task SendOnAnswerStart(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.AnswerStart), gameId, cancellationToken);
    }

    public async Task SendOnQuestionShow(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.QuestionShow), gameId, cancellationToken);
    }

    public async Task SendOnRoundEnd(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.RoundEnd), gameId, cancellationToken);
    }

    public async Task WaitForAnswerShown(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == Actions.AnswerShown, cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Actions.AnswerShown), gameId, cancellationToken);
    }

    public async Task WaitOnAsnwered(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == Actions.Answered, cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Actions.Answered), gameId, cancellationToken);
    }

    public async Task WaitForQuestionShown(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == Actions.QuestionShown, cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Actions.QuestionShown), gameId, cancellationToken);
    }

    public async Task WaitForRoundEnded(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == Actions.RoundEnded, cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Actions.RoundEnded), gameId, cancellationToken);
    }

    public async Task<IMiniGameEventService.AnswerSelection?> GetAnswer(string gameId, CancellationToken cancellationToken = default)
    {
        var players = await GetPlayersAsync(gameId, cancellationToken);

        var answer = await playerInteraction.ConsumeFirstAsync(
            condition: x => x.GameId == gameId
                && x.InteractionType == Interactions.Answer
                && x.Value != null,
        cancellationToken: cancellationToken);

        if (answer?.Value is null) return null;

        var answerPlayerId = players.FirstOrDefault(x => x.DeviceId == answer.DeviceId)?.Id; 

        return new IMiniGameEventService.AnswerSelection(answerPlayerId ?? default, answer.Value, answer.Timestamp);
    }

    private async Task<IEnumerable<Player>> GetPlayersAsync(string gameId, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(gameId, out var id)) throw new ArgumentException("Invalid gameId");
        var game = await repository.FindAsync(id, cancellationToken);
        return game.Players;
    }
}