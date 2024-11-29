using System.Text.Json.Serialization;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages.Game;
using RabbitMQ.Client;

namespace Quiz.Master.Consumers;

internal class RulesExplainConsumer : ConsumerBase<RulesExplain>
{
    private readonly IPublisher publisher;

    public RulesExplainConsumer(
        IConnection connection,
        IQueueConsumerDefinition<RulesExplain> queueDefinition,
        ILogger<RulesExplainConsumer> logger,
        JsonSerializerContext jsonSerializerContext,
        IPublisher publisher)
    : base(connection, queueDefinition, logger, jsonSerializerContext)
    {
        this.publisher = publisher;
    }

    protected override async Task ProcessMessageAsync(RulesExplain message, CancellationToken cancellationToken = default)
    {
        await Task.Delay(10_000);

        await publisher.PublishAsync(new RulesExplained(message.GameId), cancellationToken);
    }
}