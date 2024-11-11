using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Quiz.CommonLib.MessageBroker.Messages;
using RabbitMQ.Client;

namespace Quiz.CommonLib.MessageBroker.Publisher;

public class RabbitMQPublisher(IConnection connection, JsonSerializerContext jsonSerializerContext) : IPublisher
{
    private IChannel? _channel = null;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : IMessage
    {
        if (_channel == null || _channel.IsClosed)
        {
            await _semaphore.WaitAsync(cancellationToken);
            if (_channel == null || _channel.IsClosed)
            {
                _channel = await connection.CreateChannelAsync();
            }
        }

        var jsonTypeInfo = jsonSerializerContext.GetTypeInfo(typeof(T))!;
        var stringBody = JsonSerializer.Serialize(message, jsonTypeInfo);
        var body = Encoding.UTF8.GetBytes(stringBody);
        var dest = message.Destination();
        await _channel.BasicPublishAsync(dest.Exchange, dest.RoutingKey, body, cancellationToken);
    }
}