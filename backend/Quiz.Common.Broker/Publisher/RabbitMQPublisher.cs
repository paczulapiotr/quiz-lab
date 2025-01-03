using System.Text;
using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;
using RabbitMQ.Client;
using static System.Text.Json.JsonSerializer;

namespace Quiz.Common.Broker.Publisher;

public class RabbitMQPublisher(IConnection connection, IQueuePublisherDefinitionProvider queueDefinitionProvider) : IPublisher
{
    private IChannel? _channel = null;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public async Task PublishAsync<T>(T message, IQueuePublisherDefinition<T> queueDefinition, string? routingKey = null, CancellationToken cancellationToken = default) where T : IMessage
    {
        if (_channel == null || _channel.IsClosed)
        {
            await _semaphore.WaitAsync(cancellationToken);
            if (_channel == null || _channel.IsClosed)
            {
                _channel = await connection.CreateChannelAsync();
            }
        }

        var stringBody = Serialize(message);
        var body = Encoding.UTF8.GetBytes(stringBody);

        var exchangeRoutingKey = queueDefinition.MapRoutingKey(routingKey);
        await _channel.BasicPublishAsync(queueDefinition.ExchangeName, exchangeRoutingKey, body, cancellationToken);
    }

    public async Task PublishAsync<T>(T message, string? routingKey = null, CancellationToken cancellationToken = default) where T : IMessage
    {
        var queueDefinition = queueDefinitionProvider.GetPublisherDefinition<T>();

        ArgumentNullException.ThrowIfNull(queueDefinition, nameof(queueDefinition));

        await PublishAsync(message, queueDefinition, routingKey, cancellationToken);
    }
}