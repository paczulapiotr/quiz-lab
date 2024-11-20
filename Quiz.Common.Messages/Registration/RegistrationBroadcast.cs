using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.PingPong;

public record RegistrationBroadcast(string UniqueId, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class RegistrationBroadcastDefinition : QueueDefinition<PlayerRegistration>
{
    public RegistrationBroadcastDefinition(string UniqueId = "") : base(ExchangeType.Fanout, queueSufix: UniqueId)
    {
    }

    public static RegistrationBroadcastDefinition Publisher() => new();
    public static RegistrationBroadcastDefinition Consumer(string UniqueId) => new(UniqueId);

}