using System.Text;
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
    private static readonly IDictionary<string, object?> _persistantQueueDefaultArguments = new Dictionary<string, object?>
    {
        { "x-message-ttl", 1 * 60_000 }, // 1 minute
    };

    private static readonly IDictionary<string, object?> _queueDefaultArguments = new Dictionary<string, object?>
    {
        { "x-message-ttl", 1 * 60_000 }, // 1 minute
        { "x-expires", 30 * 60_000 } // 30 minutes
    };

    private string _queueSufix;
    private readonly bool _persistant;

    private string NameBase => typeof(TMessage).Name.ToLowerInvariant();

    public ExchangeType ExchangeType { get; init; }
    public string ExchangeName { get; init; }
    public Type MessageType => typeof(TMessage);
    public string QueueName { private set; get; }

    protected QueueDefinition(ExchangeType exchangeType, string queueSufix = "", bool persistant = false)
    {
        _queueSufix = queueSufix;
        this._persistant = persistant;
        ExchangeType = exchangeType;
        ExchangeName = $"{NameBase}-exchange";
        QueueName = CreateQueueName();
    }

    private string CreateQueueName(string? queueIdentifier = null)
    {
        var builder = new StringBuilder(NameBase);
        if (!string.IsNullOrWhiteSpace(_queueSufix))
        {
            builder.Append('-');
            builder.Append(_queueSufix);
        }

        if (!string.IsNullOrWhiteSpace(queueIdentifier))
        {
            builder.Append('-');
            builder.Append(queueIdentifier);
        }

        builder.Append("-queue");

        return builder.ToString();
    }

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

    public async Task<(string exchange, string queue)> RegisterConsumerAsync(IChannel channel, string? routingKey = null, CancellationToken cancellationToken = default)
    {
        var exchange = ExchangeType switch
        {
            ExchangeType.Fanout => "fanout",
            ExchangeType.Topic => "topic",
            ExchangeType.Direct => "direct",
            _ => throw new ArgumentOutOfRangeException(nameof(ExchangeType), ExchangeType, null),
        };

        QueueName = CreateQueueName(routingKey);

        await channel.ExchangeDeclareAsync(exchange: ExchangeName, type: exchange, durable: true, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
        await channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: _persistant ? _persistantQueueDefaultArguments : _queueDefaultArguments, cancellationToken: cancellationToken);
        await channel.QueueBindAsync(queue: QueueName, exchange: ExchangeName, routingKey: MapRoutingKey(routingKey), arguments: null, cancellationToken: cancellationToken);

        return (ExchangeName, QueueName);
    }

    public QueueDefinition<TMessage> ToConsumer(string queueName = "")
    {
        _queueSufix = queueName;
        return this;
    }

    public string MapRoutingKey(string? routingKey = null) => ExchangeType switch
    {
        ExchangeType.Fanout => "",
        ExchangeType.Topic => $"{NameBase}.{(string.IsNullOrWhiteSpace(routingKey) ? "#" : routingKey)}",
        ExchangeType.Direct => $"{NameBase}{(string.IsNullOrWhiteSpace(routingKey) ? "" : "." + routingKey)}",
        _ => throw new ArgumentOutOfRangeException(nameof(ExchangeType), ExchangeType, null)
    };
}