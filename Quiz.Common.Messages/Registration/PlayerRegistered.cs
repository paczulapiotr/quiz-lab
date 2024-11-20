using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages;

public record PlayerRegistered(string UniqueId, string PlayerName, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class PlayerRegisteredDefinition : QueueDefinition<PlayerRegistered>
{
    public PlayerRegisteredDefinition(string UniqueId = "") : base(ExchangeType.Fanout, queueSufix: UniqueId)
    {
    }

    public static PlayerRegisteredDefinition Publisher() => new();
    public static PlayerRegisteredDefinition Consumer(string UniqueId) => new(UniqueId);

}