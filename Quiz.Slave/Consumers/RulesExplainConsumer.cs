using System.Text.Json.Serialization;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages.Game;
using Quiz.Slave.Hubs;
using RabbitMQ.Client;

namespace Quiz.Slave.Consumers;

internal class RulesExplainConsumer : ConsumerBase<RulesExplain>
{
    private readonly ISyncHubClient syncHubClient;

    public RulesExplainConsumer(
        IConnection connection,
        ISyncHubClient syncHubClient,
        IQueueConsumerDefinition<RulesExplain> queueDefinition,
        ILogger<RulesExplainConsumer> logger,
        JsonSerializerContext jsonSerializerContext)
    : base(connection, queueDefinition, logger, jsonSerializerContext)
    {
        this.syncHubClient = syncHubClient;
    }

    protected override async Task ProcessMessageAsync(RulesExplain message, CancellationToken cancellationToken = default)
    {
        await syncHubClient.RulesExplain(message.GameId, cancellationToken);
    }
}