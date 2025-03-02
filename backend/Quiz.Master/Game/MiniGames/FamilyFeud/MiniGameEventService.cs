using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Abstract;
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

internal class MiniGameEventService: MiniGameEventServiceBase, IMiniGameEventService
{
    public MiniGameEventService(ILogger<MiniGameEventService> logger,
    IOneTimeConsumer<PlayerInteraction> playerInteraction,
    IOneTimeConsumer<MiniGameUpdate> miniGameUpdate,
    IMiniGameRepository miniGameRepository,
    IGameRepository repository,
    IPublisher publisher) : base(publisher, miniGameUpdate, miniGameRepository)
    {
        this.logger = logger;
        this.playerInteraction = playerInteraction;
        this.repository = repository;
    }
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    private readonly ILogger<MiniGameEventService> logger;
    private readonly IOneTimeConsumer<PlayerInteraction> playerInteraction;
    private readonly IGameRepository repository;
    private bool _isSetup = false;
    protected override string Type => MiniGameType.MusicGuess.ToString();

    public override async Task Initialize(string gameId, CancellationToken cancellationToken = default)
    {
        if (_isSetup) return;

        try
        {
            await _semaphore.WaitAsync(cancellationToken);
            await playerInteraction.RegisterAsync(gameId, cancellationToken);
            await base.Initialize(gameId, cancellationToken);
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

    public Task SendOnAnswerShow(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.AnswerShow, cancellationToken);

    public  Task SendOnAnswerStart(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.AnswerStart, cancellationToken);

    public Task SendOnQuestionShow(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.QuestionShow, cancellationToken);

    public Task SendOnRoundEnd(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => Send(gameId, miniGameId, Actions.RoundEnd, cancellationToken);

    public Task WaitForAnswerShown(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => WaitFor(gameId, miniGameId, Actions.AnswerShown, cancellationToken);

    public Task WaitOnAsnwered(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => WaitFor(gameId, miniGameId, Actions.Answered, cancellationToken);

    public Task WaitForQuestionShown(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => WaitFor(gameId, miniGameId, Actions.QuestionShown, cancellationToken);

    public Task WaitForRoundEnded(string gameId, string miniGameId, CancellationToken cancellationToken = default)
        => WaitFor(gameId, miniGameId, Actions.RoundEnded, cancellationToken);

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