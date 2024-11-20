using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.PingPong;

public record Ping(string Message, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class PingQueueDefinition : QueueDefinition<Ping>
{
    public PingQueueDefinition(string uniqueId = "") : base(ExchangeType.Fanout, queueSufix: uniqueId)
    {
    }

    public static PingQueueDefinition Publisher() => new();
    public static PingQueueDefinition Consumer(string UniqueId) => new(UniqueId);
}