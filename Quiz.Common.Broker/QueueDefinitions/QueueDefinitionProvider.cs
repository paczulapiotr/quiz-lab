using Quiz.Common.Broker.Messages;

namespace Quiz.Common.Broker.QueueDefinitions;

public interface IQueueDefinitionProvider
{
    QueueDefinition<T>? GetQueueDefinition<T>() where T : IMessage;
}

public class QueueDefinitionProvider : IQueueDefinitionProvider
{
    private readonly Dictionary<Type, IQueueDefinition> _queueDefinitions = new();

    public QueueDefinitionProvider(IEnumerable<IQueueDefinition> queueDefinitions)
    {
        foreach (var queueDefinition in queueDefinitions)
        {
            _queueDefinitions.Add(queueDefinition.MessageType, queueDefinition);
        }
    }

    public QueueDefinition<T>? GetQueueDefinition<T>() where T : IMessage
    {
        return (QueueDefinition<T>?)(_queueDefinitions.TryGetValue(typeof(T), out var queueDefinition) ? queueDefinition : null);
    }
}