using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using RabbitMQ.Client;

namespace Quiz.CommonLib.MessageBroker;

public class RabbitMQMessageBroker : IMessageBroker
{
    private readonly IConnection _connection;
    private IChannel? _channel = null;

    public RabbitMQMessageBroker(IConnection connection)
    {
        _connection = connection;
    }

    public async Task PublishAsync<T>(T message, JsonTypeInfo jsonTypeInfo, CancellationToken cancellationToken = default) where T : IMessage
    {
        if (_channel == null)
        {
            _channel = await _connection.CreateChannelAsync();
        }

        var stringBody = JsonSerializer.Serialize(message, jsonTypeInfo);
        var body = Encoding.UTF8.GetBytes(stringBody);
        await _channel.BasicPublishAsync(message.Exchange, message.Queue, body, cancellationToken);
    }
}