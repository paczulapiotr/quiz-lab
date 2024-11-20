using Quiz.Common.Broker.Messages;
using RabbitMQ.Client;

namespace Quiz.Common.Broker.QueueDefinitions;

public enum ExchangeType
{
    Fanout,
    Topic,
    Direct,
}


public abstract class QueueDefinition<TMessage>
: IQueueDefinition<TMessage> where TMessage : IMessage
{
    public ExchangeType ExchangeType { get; init; }
    public string ExchangeName { get; init; }

    public string QueueName { get; init; }

    public string RoutingKey { get; init; }

    protected QueueDefinition(ExchangeType exchangeType, string queueSufix = "", string routingKey = "")
    {
        var nameBase = typeof(TMessage).Name.ToLowerInvariant();
        ExchangeType = exchangeType;
        ExchangeName = $"{nameBase}-exchange";
        QueueName = string.IsNullOrWhiteSpace(queueSufix) ? $"{nameBase}-queue" : $"{nameBase}-{queueSufix}-queue";
        RoutingKey = string.IsNullOrWhiteSpace(routingKey) ? ExchangeType switch
        {
            ExchangeType.Fanout => "",
            ExchangeType.Topic => $"{nameBase}.#",
            ExchangeType.Direct => nameBase,
            _ => throw new ArgumentOutOfRangeException(nameof(ExchangeType), ExchangeType, null)
        } : routingKey;
    }

    public Type MessageType => typeof(TMessage);

    public async Task RegisterPublisherAsync(IChannel channel, CancellationToken cancellationToken = default)
    {
        var exchange = ExchangeType switch
        {
            ExchangeType.Fanout => "fanout",
            ExchangeType.Topic => "topic",
            ExchangeType.Direct => "direct",
            _ => throw new ArgumentOutOfRangeException(nameof(ExchangeType), ExchangeType, null),
        };

        await channel.ExchangeDeclareAsync(exchange: ExchangeName, type: exchange, durable: true, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
    }

    public async Task RegisterConsumerAsync(IChannel channel, CancellationToken cancellationToken = default)
    {
        var exchange = ExchangeType switch
        {
            ExchangeType.Fanout => "fanout",
            ExchangeType.Topic => "topic",
            ExchangeType.Direct => "direct",
            _ => throw new ArgumentOutOfRangeException(nameof(ExchangeType), ExchangeType, null),
        };

        await channel.ExchangeDeclareAsync(exchange: ExchangeName, type: exchange, durable: true, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
        await channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
        await channel.QueueBindAsync(queue: QueueName, exchange: ExchangeName, routingKey: RoutingKey, arguments: null, cancellationToken: cancellationToken);
    }
}