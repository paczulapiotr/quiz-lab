using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Abstract;
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

    public Task SendOnAnswered(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.Answered, cancellationToken);
  

    public  Task SendOnAnswerStart(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.AnswerStart, cancellationToken);

    public  Task SendOnPhraseSolvedPresentation(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.PhraseSolvedPresentation, cancellationToken);

    public  Task SendOnQuestionShow(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.QuestionShow, cancellationToken);

    public  Task WaitForPhraseSolvedPresented(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => WaitFor(gameId, miniGameId, Actions.PhraseSolvedPresented, cancellationToken);

    public Task WaitForQuestionShown(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => WaitFor(gameId, miniGameId, Actions.QuestionShown, cancellationToken);

    public async Task<IMiniGameEventService.AnswerSelection?> GetLetterSelection(string gameId, CancellationToken cancellationToken = default)
    {
        var answer = await _playerInteraction.ConsumeFirstAsync(
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
        var game = await _repository.FindAsync(id, cancellationToken);
        return game.Players;
    }
}