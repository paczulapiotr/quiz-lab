using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.PingPong;

public record Ping(string Message, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class PingQueueDefinition : QueueDefinition<Ping>
{
    public PingQueueDefinition() : base("ping-exchange", "ping-queue", ExchangeType.Fanout)
    {
    }
}