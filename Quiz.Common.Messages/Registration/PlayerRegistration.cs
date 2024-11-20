using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.PingPong;

public record PlayerRegistration(string UniqueId, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class PlayerRegistrationDefinition : QueueDefinition<PlayerRegistration>
{
    public PlayerRegistrationDefinition() : base(ExchangeType.Direct)
    {
    }

    public static PlayerRegistrationDefinition Publisher() => new();
    public static PlayerRegistrationDefinition Consumer() => new();
}