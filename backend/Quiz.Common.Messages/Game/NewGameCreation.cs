using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Messages.Game;

public record NewGameCreation(string GameId, string? CorrelationId = null) : MessageBase(CorrelationId)
{
}

public class NewGameCreationDefinition : QueueDefinition<NewGameCreation>
{
    public NewGameCreationDefinition(string UniqueId = "") : base(ExchangeType.Direct, queueSufix: UniqueId)
    {
    }


}