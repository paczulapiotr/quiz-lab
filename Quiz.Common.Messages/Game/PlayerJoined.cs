using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages;

public record PlayerJoined(Guid GameId, string PlayerName, string DeviceId, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class PlayerJoinedDefinition : QueueDefinition<PlayerJoined>
{
    public PlayerJoinedDefinition(string UniqueId = "") : base(ExchangeType.Fanout, queueSufix: UniqueId)
    {
    }

    public static PlayerJoinedDefinition Publisher() => new();
    public static PlayerJoinedDefinition Consumer(string UniqueId) => new(UniqueId);

}