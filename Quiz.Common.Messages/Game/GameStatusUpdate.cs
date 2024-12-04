using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.Game;

public enum GameStatus
{
    GameCreated = 0,
    GameJoined,
    GameStarting,
    GameStarted,
    RulesExplaining,
    RulesExplained,
    RoundStarting,
    RoundStarted,
    RoundEnding,
    RoundEnded,
    GameEnding,
    GameEnded,
}

public record GameStatusUpdate(string GameId, GameStatus Status, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class GameStatusUpdateDefinition : QueueDefinition<GameStatusUpdate>
{
    public GameStatusUpdateDefinition(string UniqueId = "") : base(ExchangeType.Fanout, queueSufix: UniqueId)
    {
    }

    public static GameStatusUpdateDefinition Publisher() => new();
    public static GameStatusUpdateDefinition Consumer(string UniqueId) => new(UniqueId);

}