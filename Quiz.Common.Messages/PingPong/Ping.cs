using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.PingPong;

public record Ping(string Message) : IMessage
{
}

public class PingQueueDefinition : QueueDefinition<Ping>
{
    public PingQueueDefinition() : base("ping-exchange", "ping-queue", ExchangeType.Fanout)
    {
    }
}