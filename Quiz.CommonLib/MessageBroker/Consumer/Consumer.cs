using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.Logging;
using Quiz.CommonLib.MessageBroker;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public abstract class Consumer<TMessage>(IChannel channel, ILogger logger, JsonTypeInfo<TMessage> jsonTypeInfo) : IConsumer<TMessage>
where TMessage : IMessage
{
    public async Task ConsumeAsync(TMessage message, CancellationToken cancellationToken = default)
    {
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                logger.LogDebug($"[{GetType().Name}] received message: {messageJson}");
                var message = JsonSerializer.Deserialize(messageJson, jsonTypeInfo);
                await ProcessMessageAsync(message!, cancellationToken);
                await channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                await channel.BasicNackAsync(ea.DeliveryTag, false, true, cancellationToken);
            }
            finally
            {
                stopwatch.Stop();
                logger.LogDebug($"[{GetType().Name}] processed message in {stopwatch.ElapsedMilliseconds}ms");
            }
        };

        await channel.BasicConsumeAsync(message.Queue, false, consumer, cancellationToken);
    }

    protected abstract Task ProcessMessageAsync(TMessage message, CancellationToken cancellationToken = default);
}