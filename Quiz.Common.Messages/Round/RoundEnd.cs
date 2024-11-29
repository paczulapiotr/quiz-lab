using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.Round;

public record RoundEnd(string GameId, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class RoundEndDefinition : QueueDefinition<RoundEnd>
{
    public RoundEndDefinition(string UniqueId = "") : base(ExchangeType.Fanout, queueSufix: UniqueId)
    {
    }

    public static RoundEndDefinition Publisher() => new();
    public static RoundEndDefinition Consumer(string UniqueId) => new(UniqueId);

}