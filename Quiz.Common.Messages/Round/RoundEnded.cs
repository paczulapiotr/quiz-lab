using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.Round;

public record RoundEnded(string GameId, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class RoundEndedDefinition : QueueDefinition<RoundEnded>
{
    public RoundEndedDefinition() : base(ExchangeType.Direct)
    {
    }

    public static RoundEndedDefinition Publisher() => new();
    public static RoundEndedDefinition Consumer() => new();

}