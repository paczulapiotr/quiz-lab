using System.Text;
using Microsoft.Extensions.Logging;
using Quiz.Common.Broker.JsonSerializer;
using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Quiz.Common.Broker.Consumer;

public class OneTimeConsumer<TMessage> : IOneTimeConsumer<TMessage>
where TMessage : IMessage
{
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    private readonly IConnection _connection;
    protected readonly IQueueConsumerDefinition<TMessage> _queueDefinition;
    protected IChannel? _channel = null;
    protected readonly IJsonSerializer jsonSerializer;
    protected readonly ILogger logger;

    public OneTimeConsumer(IConnection connection, IQueueConsumerDefinition<TMessage> queueDefinition, ILogger logger, IJsonSerializer jsonSerializer)
    {
        _connection = connection;
        _queueDefinition = queueDefinition;
        this.logger = logger;
        this.jsonSerializer = jsonSerializer;
    }

    protected async Task InitChannel(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        if (_channel == null || _channel.IsClosed)
        {
            _channel = await _connection.CreateChannelAsync();
        }
        _semaphore.Release();
    }

    public async Task<TMessage> ConsumeFirstAsync(Func<TMessage, CancellationToken, Task>? callback = null, CancellationToken cancellationToken = default)
    {
        await InitChannel(cancellationToken);
        var tcs = new TaskCompletionSource<TMessage>();

        var consumer = await CreateAsyncConsumer(async (message, token) =>
        {
            if (callback is not null)
            {
                await callback(message, token);
            }

            tcs.SetResult(message);
        },
        tcs.SetException,
        cancellationToken: cancellationToken);

        var consumerTag = await _channel!.BasicConsumeAsync(_queueDefinition.QueueName, false, consumer);

        var result = await tcs.Task;
        await _channel!.BasicCancelAsync(consumerTag);

        return result;
    }

    protected async Task<AsyncEventingBasicConsumer> CreateAsyncConsumer(Func<TMessage, CancellationToken, Task> callback, Action<Exception>? onException = null, CancellationToken cancellationToken = default)
    {
        if (_channel is null)
        {
            throw new InvalidOperationException("Channel is not initialized");
        }

        var consumer = new AsyncEventingBasicConsumer(_channel);

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
                var message = jsonSerializer.Deserialize<TMessage>(messageJson);
                messageId = message?.MessageId;
                correlationId = message?.CorrelationId;
                logger.LogTrace($"[{correlationId}/{messageId}] received message {typeof(TMessage)}: {messageJson}");

                await callback(message!, cancellationToken);

                await _channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                await _channel.BasicNackAsync(ea.DeliveryTag, false, false, cancellationToken); // TODO: requeue mechanism
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

    public void Dispose()
    {
        _channel?.Dispose();
    }

}