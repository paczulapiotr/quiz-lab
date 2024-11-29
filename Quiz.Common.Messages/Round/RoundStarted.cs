using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.Round;

public record RoundStarted(string GameId, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class RoundStartedDefinition : QueueDefinition<RoundStarted>
{
    public RoundStartedDefinition() : base(ExchangeType.Fanout)
    {
    }

    public static RoundStartedDefinition Publisher() => new();
    public static RoundStartedDefinition Consumer() => new();

}