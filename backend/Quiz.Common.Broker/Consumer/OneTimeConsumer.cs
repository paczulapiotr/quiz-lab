using System.Text;
using Microsoft.Extensions.Logging;
using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using static System.Text.Json.JsonSerializer;

namespace Quiz.Common.Broker.Consumer;

public class OneTimeConsumer<TMessage> : IOneTimeConsumer<TMessage>
where TMessage : class, IMessage
{
    protected readonly IConnection _connection;
    protected readonly IQueueConsumerDefinition<TMessage> _queueDefinition;
    protected readonly ILogger logger;
    
    public bool IsConnected => _connection.IsOpen;

    public OneTimeConsumer(IConnection connection, IQueueConsumerDefinition<TMessage> queueDefinition, ILogger logger)
    {
        _connection = connection;
        _queueDefinition = queueDefinition;
        this.logger = logger;
    }

    protected async Task<IChannel> CreateChannelAsync() {
        var channel =  await _connection.CreateChannelAsync();
        channel.ChannelShutdownAsync += async (sender, args) => {
            
            logger.LogInformation($"Channel shutdown for exchange: {_queueDefinition.ExchangeName}, queue: {_queueDefinition.QueueName}, reason: {args.ReplyText}");
            await Task.CompletedTask;
        };
        return channel;
    }

    public async Task<TMessage?> ConsumeFirstAsync(Func<TMessage, CancellationToken, Task>? callback = null, Func<TMessage, bool>? condition = null, CancellationToken cancellationToken = default)
    {
        using var channel = await CreateChannelAsync();

        while (!cancellationToken.IsCancellationRequested)
        {
            var message = await ConsumeAsync(channel, callback, condition, cancellationToken: cancellationToken);
            if (message is null || condition is null || condition(message))
            {
                return message;
            }
        }
        return null;
    }

    private async Task<TMessage?> ConsumeAsync(IChannel channel, Func<TMessage, CancellationToken, Task>? callback = null, Func<TMessage, bool>? condition = null, CancellationToken cancellationToken = default)
    {
        var tcs = new TaskCompletionSource<TMessage?>();

        cancellationToken.Register(() =>
        {
            logger.LogWarning($"Task cancelled while processing message of type {typeof(TMessage)}");
            tcs.TrySetCanceled();
        });

        var consumer = await CreateAsyncConsumer(channel, async (message, token) =>
        {
            if (condition is not null && !condition(message))
            {
                return;
            }

            if (callback is not null)
            {
                await callback(message, token);
            }
            tcs.SetResult(message);
        },
        tcs.SetException,
        cancellationToken: cancellationToken);

        string? consumerTag = null;

        try
        {
            consumerTag = await channel.BasicConsumeAsync(_queueDefinition.QueueName, false, consumer, cancellationToken);
            logger.LogInformation("Created consumer tag: {0} for message: {1}", consumerTag, typeof(TMessage).Name);
            var result = await tcs.Task;
            logger.LogInformation("Message consumed of type {0}", typeof(TMessage).Name);
            await channel.BasicCancelAsync(consumerTag);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error while processing message of type {typeof(TMessage).Name}", ex);
            if (consumerTag is not null)
            {
                await channel.BasicCancelAsync(consumerTag);
            }
            return null;
        }
    }

    protected async Task<AsyncEventingBasicConsumer> CreateAsyncConsumer(IChannel channel, Func<TMessage, CancellationToken, Task> callback, Action<Exception>? onException = null, CancellationToken cancellationToken = default)
    {
        var consumer = new AsyncEventingBasicConsumer(channel);

        await consumer.Channel.BasicQosAsync(0, 1, false, cancellationToken);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            string? messageId = null;
            string? correlationId = null;

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                var message = Deserialize<TMessage>(messageJson);
                messageId = message?.MessageId;
                correlationId = message?.CorrelationId;
                logger.LogTrace($"[{correlationId}/{messageId}] received message {typeof(TMessage)}: {messageJson}");

                await callback(message!, cancellationToken);

                await channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                await channel.BasicNackAsync(ea.DeliveryTag, false, false, cancellationToken); // TODO: requeue mechanism
                onException?.Invoke(ex);
            }
            finally
            {
                stopwatch.Stop();
                logger.LogTrace($"[{correlationId}/{messageId}] processed message {typeof(TMessage)} in {stopwatch.ElapsedMilliseconds}ms");
            }
        };
        return consumer;
    }

    public async Task RegisterAsync(string? routingKey = null, CancellationToken cancellationToken = default)
    {
        using var channel = await CreateChannelAsync();
        var (exchange, queue) = await _queueDefinition.RegisterConsumerAsync(channel, routingKey, cancellationToken);
        logger.LogInformation($"Consumer registered for exchange: '{exchange}' and queue: '{queue}'");
    }
}