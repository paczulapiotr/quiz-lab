using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.Game;

// MiniGameNotification is used for client notifications
public record MiniGameNotification(string GameId, string MiniGameType, string Action, Dictionary<string, string>? Metadata = null, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class MiniGameNotificationDefinition : QueueDefinition<MiniGameNotification>
{
    public MiniGameNotificationDefinition(string UniqueId = "") : base(ExchangeType.Fanout, queueSufix: UniqueId)
    {
    }
}

public class MiniGameNotificationDefinitionSingleDefinition : MiniGameNotificationDefinition
{
    public MiniGameNotificationDefinitionSingleDefinition(string UniqueId = "") : base(UniqueId + "-single")
    {
    }
}