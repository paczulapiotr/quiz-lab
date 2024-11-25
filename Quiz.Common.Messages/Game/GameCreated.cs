using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages;

public record GameCreated(string GameId, uint GameSize, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class GameCreatedDefinition : QueueDefinition<GameCreated>
{
    public GameCreatedDefinition(string UniqueId = "") : base(ExchangeType.Fanout, queueSufix: UniqueId)
    {
    }

    public static GameCreatedDefinition Publisher() => new();
    public static GameCreatedDefinition Consumer(string UniqueId) => new(UniqueId);

}