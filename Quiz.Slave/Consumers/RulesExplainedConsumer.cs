using System.Text.Json.Serialization;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages.Game;
using Quiz.Slave.Hubs;
using RabbitMQ.Client;

namespace Quiz.Slave.Consumers;

internal class RulesExplainedConsumer : ConsumerBase<RulesExplained>
{
    private readonly ISyncHubClient syncHubClient;

    public RulesExplainedConsumer(
        IConnection connection,
        ISyncHubClient syncHubClient,
        IQueueConsumerDefinition<RulesExplained> queueDefinition,
        ILogger<RulesExplainedConsumer> logger,
        JsonSerializerContext jsonSerializerContext)
    : base(connection, queueDefinition, logger, jsonSerializerContext)
    {
        this.syncHubClient = syncHubClient;
    }

    protected override async Task ProcessMessageAsync(RulesExplained message, CancellationToken cancellationToken = default)
    {
        await syncHubClient.RulesExplain(message.GameId, cancellationToken);
    }
}