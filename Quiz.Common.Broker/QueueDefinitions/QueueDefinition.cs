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

    private static readonly IDictionary<string, object?> _queueDefaultArguments = new Dictionary<string, object?>
    {
        { "x-message-ttl", 60000 },
        { "x-expires", 60000 }
    };

    public ExchangeType ExchangeType { get; init; }
    public string ExchangeName { get; init; }

    public string QueueName { get; private set; } = "";

    public string RoutingKey { get; init; }
    private string NameBase => typeof(TMessage).Name.ToLowerInvariant();

    protected QueueDefinition(ExchangeType exchangeType, string queueSufix = "", string routingKey = "")
    {
        ExchangeType = exchangeType;
        ExchangeName = $"{NameBase}-exchange";
        SetQueueName(queueSufix);
        RoutingKey = string.IsNullOrWhiteSpace(routingKey) ? ExchangeType switch
        {
            ExchangeType.Fanout => "",
            ExchangeType.Topic => $"{NameBase}.#",
            ExchangeType.Direct => NameBase,
            _ => throw new ArgumentOutOfRangeException(nameof(ExchangeType), ExchangeType, null)
        } : routingKey;
    }

    private void SetQueueName(string? queueName)
    {
        QueueName = string.IsNullOrWhiteSpace(queueName) ? $"{NameBase}-queue" : $"{NameBase}-{queueName}-queue";
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
        await channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: _queueDefaultArguments, cancellationToken: cancellationToken);
        await channel.QueueBindAsync(queue: QueueName, exchange: ExchangeName, routingKey: RoutingKey, arguments: null, cancellationToken: cancellationToken);
    }

    public QueueDefinition<TMessage> ToConsumer(string queueName = "")
    {
        SetQueueName(queueName);
        return this;
    }
}