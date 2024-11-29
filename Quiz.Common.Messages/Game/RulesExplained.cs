using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.Game;

public record RulesExplained(string GameId, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class RulesExplainedDefinition : QueueDefinition<RulesExplained>
{
    public RulesExplainedDefinition(string UniqueId = "") : base(ExchangeType.Fanout, queueSufix: UniqueId)
    {
    }

    public static RulesExplainedDefinition Publisher() => new();
    public static RulesExplainedDefinition Consumer(string UniqueId) => new(UniqueId);

}