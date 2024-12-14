using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.Game;

public record PlayerInteraction(string GameId, string PlayerId, string InteractionType, string? Value, Dictionary<string, string>? Data, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class PlayerInteractionDefinition : QueueDefinition<PlayerInteraction>
{
    public PlayerInteractionDefinition() : base(ExchangeType.Direct)
    {
    }
}