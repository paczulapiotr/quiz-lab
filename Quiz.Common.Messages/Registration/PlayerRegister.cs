using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.PingPong;

public record PlayerRegister(string UniqueId, string PlayerName, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class PlayerRegisterDefinition : QueueDefinition<PlayerRegister>
{
    public PlayerRegisterDefinition() : base(ExchangeType.Direct)
    {
    }

    public static PlayerRegisterDefinition Publisher() => new();
    public static PlayerRegisterDefinition Consumer() => new();
}