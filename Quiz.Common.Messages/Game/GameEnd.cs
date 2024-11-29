using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.Game;

public record GameEnd(string GameId, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class GameEndDefinition : QueueDefinition<GameEnd>
{
    public GameEndDefinition(string UniqueId = "") : base(ExchangeType.Fanout, queueSufix: UniqueId)
    {
    }

    public static GameEndDefinition Publisher() => new();
    public static GameEndDefinition Consumer(string UniqueId) => new(UniqueId);

}