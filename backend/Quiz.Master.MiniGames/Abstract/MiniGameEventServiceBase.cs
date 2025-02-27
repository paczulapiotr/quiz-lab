using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;
using Quiz.Master.Game.MiniGames;

namespace Quiz.Master.MiniGames.Abstract;

public abstract class MiniGameEventServiceBase
{
    protected readonly IPublisher _publisher;
    protected readonly IOneTimeConsumer<MiniGameUpdate> _miniGameUpdate;
    protected readonly IMiniGameRepository _miniGameRepository;

    protected MiniGameEventServiceBase(
        IPublisher publisher,
        IOneTimeConsumer<MiniGameUpdate> miniGameUpdate,
        IMiniGameRepository miniGameRepository)
    {
        _publisher = publisher;
        _miniGameUpdate = miniGameUpdate;
        _miniGameRepository = miniGameRepository;
    }

    abstract protected string Type { get; }

    public virtual async Task Initialize(string gameId, CancellationToken cancellationToken)
    {
        await _miniGameUpdate.RegisterAsync(gameId, cancellationToken);
    }

    protected async Task<string> Send(string gameId, string miniGameId, string action, CancellationToken cancellationToken)
    {
        if (Guid.TryParse(miniGameId, out var id))
        {
            await _miniGameRepository.UpdateMiniGameStatusAsync(id, action, cancellationToken);
        }

        await _publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: action), gameId, cancellationToken);
        return action;
    }

    protected async Task<string> WaitFor(string gameId, string miniGameId, string action, CancellationToken cancellationToken)
    {
        await _miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == action, cancellationToken: cancellationToken);
        if (Guid.TryParse(miniGameId, out var id))
        {
            await _miniGameRepository.UpdateMiniGameStatusAsync(id, action, cancellationToken);
        }
        await _publisher.PublishAsync(new MiniGameNotification(gameId, Type, Action: action), gameId, cancellationToken);

        return action;
    }
}