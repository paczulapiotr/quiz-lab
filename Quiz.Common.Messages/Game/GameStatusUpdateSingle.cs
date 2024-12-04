using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.Game;


public record GameStatusUpdateSingle(string GameId, GameStatus Status, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class GameStatusUpdateSingleDefinition : QueueDefinition<GameStatusUpdateSingle>
{
    public GameStatusUpdateSingleDefinition(string UniqueId = "") : base(ExchangeType.Fanout, queueSufix: UniqueId + "-single")
    {
    }

    public static GameStatusUpdateSingleDefinition Publisher() => new();
    public static GameStatusUpdateSingleDefinition Consumer(string UniqueId) => new(UniqueId);

}