using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.Round;

public record RoundStart(string GameId, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class RoundStartDefinition : QueueDefinition<RoundStart>
{
    public RoundStartDefinition(string UniqueId = "") : base(ExchangeType.Fanout, queueSufix: UniqueId)
    {
    }

    public static RoundStartDefinition Publisher() => new();
    public static RoundStartDefinition Consumer(string UniqueId) => new(UniqueId);

}