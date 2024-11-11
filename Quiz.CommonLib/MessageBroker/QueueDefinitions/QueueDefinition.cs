using RabbitMQ.Client;

namespace Quiz.CommonLib.MessageBroker.QueueDefinitions;


public class QueueDefinition(string ExchangeName, string QueueName, string ExchangeType, string RoutingKey = "") : IQueueDefinition
{
    public async Task RegisterAsync(IChannel channel, CancellationToken cancellationToken = default)
    {
        await channel.ExchangeDeclareAsync(exchange: ExchangeName, type: ExchangeType, durable: true, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
        await channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
        await channel.QueueBindAsync(queue: QueueName, exchange: ExchangeName, routingKey: RoutingKey, arguments: null, cancellationToken: cancellationToken);
    }
}