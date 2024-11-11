using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.Logging;
using Quiz.CommonLib.MessageBroker.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Quiz.CommonLib.MessageBroker.Consumer;

public abstract class ConsumerBase<TMessage>(IChannel channel, ILogger logger, JsonSerializerContext jsonSerializerContext) : IConsumer<TMessage>, IDisposable
where TMessage : IMessage
{
    public static Type GetMessageTypeInfo() => typeof(TMessage);
    public async Task ConsumeAsync(string queueName, CancellationToken cancellationToken = default)
    {
        var jsonTypeInfo = jsonSerializerContext.GetTypeInfo(typeof(TMessage)) as JsonTypeInfo<TMessage>;
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                logger.LogInformation($"[{GetType().Name}] received message: {messageJson}");
                var message = JsonSerializer.Deserialize(messageJson, jsonTypeInfo!);
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
                logger.LogInformation($"[{GetType().Name}] processed message in {stopwatch.ElapsedMilliseconds}ms");
            }
        };

        await channel.BasicConsumeAsync(queueName, false, consumer, cancellationToken);
    }

    public void Dispose()
    {
        channel.Dispose();
    }

    protected abstract Task ProcessMessageAsync(TMessage message, CancellationToken cancellationToken = default);
}