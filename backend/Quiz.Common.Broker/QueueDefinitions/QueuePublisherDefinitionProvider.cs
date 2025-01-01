using Quiz.Common.Broker.Messages;

namespace Quiz.Common.Broker.QueueDefinitions;

public interface IQueuePublisherDefinitionProvider
{
    IQueuePublisherDefinition<T>? GetPublisherDefinition<T>() where T : IMessage;
}

public class QueuePublisherDefinitionProvider : IQueuePublisherDefinitionProvider
{
    private readonly Dictionary<Type, IQueuePublisherDefinition> _publisherDefinitions = new();
    private readonly Dictionary<Type, IQueueConsumerDefinition> _consumerDefinitions = new();

    public QueuePublisherDefinitionProvider(
        IEnumerable<IQueuePublisherDefinition> queuePublisherDefinitions)
    {
        foreach (var def in queuePublisherDefinitions)
        {
            _publisherDefinitions.Add(def.MessageType, def);
        }
    }

    public IQueuePublisherDefinition<T>? GetPublisherDefinition<T>() where T : IMessage
    {
        return (IQueuePublisherDefinition<T>?)(_publisherDefinitions.TryGetValue(typeof(T), out var def) ? def : null);
    }

}