using System.Text.Json.Serialization.Metadata;

namespace Quiz.CommonLib.MessageBroker;

public interface IMessageBroker
{
    Task PublishAsync<T>(T message, JsonTypeInfo jsonTypeInfo, CancellationToken cancellationToken = default) where T : IMessage;
}