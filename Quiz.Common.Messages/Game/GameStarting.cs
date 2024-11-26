using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages;

public record GameStarting(string GameId, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class GameStartingDefinition : QueueDefinition<GameStarting>
{
    public GameStartingDefinition(string UniqueId = "") : base(ExchangeType.Fanout, queueSufix: UniqueId)
    {
    }

    public static GameStartingDefinition Publisher() => new();
    public static GameStartingDefinition Consumer(string UniqueId) => new(UniqueId);

}