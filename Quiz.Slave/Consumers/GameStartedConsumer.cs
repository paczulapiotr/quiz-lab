using System.Text.Json.Serialization;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages;
using Quiz.Slave.Hubs;
using RabbitMQ.Client;

namespace Quiz.Slave.Consumers;

internal class GameStartedConsumer : ConsumerBase<GameStarted>
{
    private readonly ISyncHubClient syncHubClient;

    public GameStartedConsumer(
        IConnection connection,
        ISyncHubClient syncHubClient,
        IQueueConsumerDefinition<GameStarted> queueDefinition,
        ILogger<GameStartedConsumer> logger,
        JsonSerializerContext jsonSerializerContext)
    : base(connection, queueDefinition, logger, jsonSerializerContext)
    {
        this.syncHubClient = syncHubClient;
    }

    protected override async Task ProcessMessageAsync(GameStarted message, CancellationToken cancellationToken = default)
    {
        await syncHubClient.GameStarted(message.GameId, cancellationToken);
    }
}