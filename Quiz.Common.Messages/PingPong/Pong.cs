using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.PingPong;

public record Pong(string Message, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class PongQueueDefinition : QueueDefinition<Pong>
{
    public PongQueueDefinition(string uniqueId = "") : base(ExchangeType.Fanout, queueSufix: uniqueId)
    {
    }

    public static PongQueueDefinition Publisher() => new();
    public static PongQueueDefinition Consumer(string UniqueId) => new(UniqueId);
}