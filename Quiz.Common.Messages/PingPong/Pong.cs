using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.PingPong;

public record Pong(string Message, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class PongQueueDefinition : QueueDefinition<Pong>
{
    public PongQueueDefinition() : base("pong-exchange", "pong-queue", ExchangeType.Fanout)
    {
    }
}