using System.Text.Json.Serialization;
using Quiz.CommonLib.MessageBroker;
using Quiz.CommonLib.MessageBroker.Consumer;
using Quiz.CommonLib.MessageBroker.Messages;
using RabbitMQ.Client;

public class ConsumerHostedService(
    ILogger<ConsumerHostedService> logger,
    IConnection connection,
    JsonSerializerContext jsonSerializerContext) : IHostedService
{
    private IChannel? _channel = null;
    private TestConsumer? _consumer = null;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting consumer hosted service");
        _channel = await connection.CreateChannelAsync();
        _consumer = new TestConsumer(_channel, logger, jsonSerializerContext);
        await _consumer.ConsumeAsync(queueName: "quiz-queue", cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _channel?.Dispose();
        _consumer?.Dispose();
        return Task.CompletedTask;
    }
}

public record TestMessage(string Message, int Number, DateTime Timestamp) : IMessage
{
    public MessageDest Destination() => new MessageDest("quiz-exchange");

    public MessageSrc Source() => new MessageSrc("quiz-queue");
}

public class TestConsumer : ConsumerBase<TestMessage>
{
    public TestConsumer(IChannel channel, ILogger logger, JsonSerializerContext jsonSerializerContext)
    : base(channel, logger, jsonSerializerContext)
    {
    }

    protected override async Task ProcessMessageAsync(TestMessage message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Start processing message: {message.Message}, {message.Number}, {message.Timestamp}");
        await Task.Delay(50, cancellationToken);
        Console.WriteLine($"Finished processing message: {message.Message}, {message.Number}, {message.Timestamp}");
    }
}