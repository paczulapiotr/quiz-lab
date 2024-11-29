using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.Game;

public record RulesExplain(string GameId, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class RulesExplainDefinition : QueueDefinition<RulesExplain>
{
    public RulesExplainDefinition(string UniqueId = "") : base(ExchangeType.Fanout, queueSufix: UniqueId)
    {
    }

    public static RulesExplainDefinition Publisher() => new();
    public static RulesExplainDefinition Consumer(string UniqueId) => new(UniqueId);

}