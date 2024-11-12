using Quiz.Common.Broker.Messages;
using RabbitMQ.Client;

namespace Quiz.Common.Broker.QueueDefinitions;

public enum ExchangeType
{
    Fanout,
    Topic,
}


public abstract class QueueDefinition<TMessage>(string exchangeName, string queueName, ExchangeType exchangeType, string routingKey = "")
: IQueueDefinition<TMessage> where TMessage : IMessage
{
    public string ExchangeName => exchangeName;

    public string QueueName => queueName;

    public string RoutingKey => routingKey;

    public Type MessageType => typeof(TMessage);

    public async Task RegisterAsync(IChannel channel, CancellationToken cancellationToken = default)
    {
        var exchange = exchangeType switch
        {
            ExchangeType.Fanout => "fanout",
            ExchangeType.Topic => "topic",
            _ => throw new ArgumentOutOfRangeException(nameof(exchangeType), exchangeType, null),
        };

        await channel.ExchangeDeclareAsync(exchange: ExchangeName, type: exchange, durable: true, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
        await channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
        await channel.QueueBindAsync(queue: QueueName, exchange: ExchangeName, routingKey: RoutingKey, arguments: null, cancellationToken: cancellationToken);
    }

}