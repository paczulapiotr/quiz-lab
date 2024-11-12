using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.Logging;
using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Quiz.Common.Broker.Consumer;

public abstract class ConsumerBase<TMessage>(IConnection connection, IQueueDefinition<TMessage> queueDefinition, ILogger logger, JsonSerializerContext jsonSerializerContext) : IConsumer
where TMessage : IMessage
{
    private IChannel? _channel = null;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public async Task ConsumeAsync(CancellationToken cancellationToken = default)
    {
        if (_channel == null || _channel.IsClosed)
        {
            await _semaphore.WaitAsync(cancellationToken);
            if (_channel == null || _channel.IsClosed)
            {
                _channel = await connection.CreateChannelAsync();
            }
        }

        var jsonTypeInfo = jsonSerializerContext.GetTypeInfo(typeof(TMessage)) as JsonTypeInfo<TMessage>;
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            string? messageId = null;
            string? correlationId = null;

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize(messageJson, jsonTypeInfo!);
                messageId = message?.MessageId;
                correlationId = message?.CorrelationId;

                logger.LogInformation($"[{correlationId}/{messageId}] received message: {messageJson}");
                await ProcessMessageAsync(message!, cancellationToken);
                await _channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                await _channel.BasicNackAsync(ea.DeliveryTag, false, false, cancellationToken); // TODO: requeue mechanism
            }
            finally
            {
                stopwatch.Stop();
                logger.LogInformation($"[{correlationId}/{messageId}] processed message in {stopwatch.ElapsedMilliseconds}ms");
            }
        };

        await _channel.BasicConsumeAsync(queueDefinition.QueueName, false, consumer, cancellationToken);
    }

    public void Dispose()
    {
        _channel?.Dispose();
    }

    protected abstract Task ProcessMessageAsync(TMessage message, CancellationToken cancellationToken = default);
}