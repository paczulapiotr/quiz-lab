using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.Game;

public enum GameStatus
{
    GameCreated = 1,
    GameJoined,
    GameStarting,
    GameStarted,
    RulesExplaining,
    RulesExplained,
    MiniGameStarting,
    MiniGameStarted,
    MiniGameEnding,
    MiniGameEnded,
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


}

public class GameStatusUpdateSingleDefinition : GameStatusUpdateDefinition
{
    public GameStatusUpdateSingleDefinition(string UniqueId = "") : base(UniqueId + "-single")
    {
    }
}