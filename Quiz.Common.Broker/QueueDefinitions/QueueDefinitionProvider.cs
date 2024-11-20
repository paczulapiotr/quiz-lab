using Quiz.Common.Broker.Messages;

namespace Quiz.Common.Broker.QueueDefinitions;

public interface IQueueDefinitionProvider
{
    IQueuePublisherDefinition<T>? GetPublisherDefinition<T>() where T : IMessage;
    IQueueConsumerDefinition<T>? GetConsumerDefinition<T>() where T : IMessage;
}

public class QueueDefinitionProvider : IQueueDefinitionProvider
{
    private readonly Dictionary<Type, IQueuePublisherDefinition> _publisherDefinitions = new();
    private readonly Dictionary<Type, IQueueConsumerDefinition> _consumerDefinitions = new();

    public QueueDefinitionProvider(
        IEnumerable<IQueueConsumerDefinition> queueConsumerDefinitions,
        IEnumerable<IQueuePublisherDefinition> queuePublisherDefinitions)
    {
        foreach (var def in queuePublisherDefinitions)
        {
            _publisherDefinitions.Add(def.MessageType, def);
        }

        foreach (var def in queueConsumerDefinitions)
        {
            _consumerDefinitions.Add(def.MessageType, def);
        }
    }

    public IQueuePublisherDefinition<T>? GetPublisherDefinition<T>() where T : IMessage
    {
        return (IQueuePublisherDefinition<T>?)(_publisherDefinitions.TryGetValue(typeof(T), out var def) ? def : null);
    }

    public IQueueConsumerDefinition<T>? GetConsumerDefinition<T>() where T : IMessage
    {
        return (IQueueConsumerDefinition<T>?)(_consumerDefinitions.TryGetValue(typeof(T), out var def) ? def : null);
    }
}