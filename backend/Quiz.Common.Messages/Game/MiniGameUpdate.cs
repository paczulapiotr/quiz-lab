using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.Game;

// MiniGameUpdate is used for game engine updates
public record MiniGameUpdate(string GameId, string Action, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class MiniGameUpdateDefinition : QueueDefinition<MiniGameUpdate>
{
    public MiniGameUpdateDefinition(string UniqueId = "", bool persistant = true) : base(ExchangeType.Topic, queueSufix: UniqueId, persistant)
    {
    }
}

public class MiniGameUpdateSingleDefinition : MiniGameUpdateDefinition
{
    public MiniGameUpdateSingleDefinition(string UniqueId = "") : base(UniqueId + "-single", persistant: false)
    {
    }
}