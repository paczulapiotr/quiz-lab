using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages;

public record GameStarted(string GameId, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class GameStartedDefinition : QueueDefinition<GameStarted>
{
    public GameStartedDefinition(string UniqueId = "") : base(ExchangeType.Fanout, queueSufix: UniqueId)
    {
    }

    public static GameStartedDefinition Publisher() => new();
    public static GameStartedDefinition Consumer(string UniqueId) => new(UniqueId);

}