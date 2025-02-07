using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Handlers.LettersAndPhrases;
using Quiz.Master.Persistance.Repositories.Abstract;

namespace Quiz.Master.Game.MiniGames.LettersAndPhrases;

internal static class Actions
{
    public static string QuestionShow = "Letters.QuestionShow";
    public static string AnswerStart = "Letters.AnswerStart";
    public static string Answered = "Letters.Answered";
    public static string PhraseSolvedPresentation = "Letters.PhraseSolvedPresentation";
    public static string PhraseSolvedPresented = "Letters.PhraseSolvedPresented";
    public static string QuestionShown = "Letters.QuestionShown";
    
}

internal static class Interactions
{
    public static string Answer = "Letters.Answer";
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

    public async Task SendOnAnswered(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.Answered), gameId, cancellationToken);
    }

    public async Task SendOnAnswerStart(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.AnswerStart), gameId, cancellationToken);
    }

    public async Task SendOnPhraseSolvedPresentation(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.PhraseSolvedPresentation), gameId, cancellationToken);
    }

    public async Task SendOnQuestionShow(string gameId, CancellationToken cancellationToken = default)
    {
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: Actions.QuestionShow), gameId, cancellationToken);

    }

    public async Task WaitForPhraseSolvedPresented(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == Actions.PhraseSolvedPresented, cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Actions.PhraseSolvedPresented), gameId, cancellationToken);
    }

    public async Task WaitForQuestionShown(string gameId, CancellationToken cancellationToken = default)
    {
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == Actions.QuestionShown, cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, Type, Actions.QuestionShown), gameId, cancellationToken);
    }

    public async Task<IMiniGameEventService.AnswerSelection?> GetLetterSelection(string gameId, CancellationToken cancellationToken = default)
    {
        var answer = await playerInteraction.ConsumeFirstAsync(
            condition: x => x.GameId == gameId
                && x.InteractionType == Interactions.Answer
                && x.Value != null,
        cancellationToken: cancellationToken);

        if (answer?.Value is null) return null;

        var players = await GetPlayersAsync(gameId, cancellationToken);
        var playerId = players.First(x => x.DeviceId == answer.DeviceId).Id;

        return new IMiniGameEventService.AnswerSelection(playerId, answer.Value.ToCharArray()[0], answer.Timestamp);
    }

    private async Task<IEnumerable<Player>> GetPlayersAsync(string gameId, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(gameId, out var id)) throw new ArgumentException("Invalid gameId");
        var game = await repository.FindAsync(id, cancellationToken);
        return game.Players;
    }
}