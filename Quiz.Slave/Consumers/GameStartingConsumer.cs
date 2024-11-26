using System.Text.Json.Serialization;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages;
using Quiz.Slave.Hubs;
using RabbitMQ.Client;

namespace Quiz.Slave.Consumers;

internal class GameStartingConsumer : ConsumerBase<GameStarting>
{
    private readonly ISyncHubClient syncHubClient;

    public GameStartingConsumer(
        IConnection connection,
        ISyncHubClient syncHubClient,
        IQueueConsumerDefinition<GameStarting> queueDefinition,
        ILogger<GameStartingConsumer> logger,
        JsonSerializerContext jsonSerializerContext)
    : base(connection, queueDefinition, logger, jsonSerializerContext)
    {
        this.syncHubClient = syncHubClient;
    }

    protected override async Task ProcessMessageAsync(GameStarting message, CancellationToken cancellationToken = default)
    {
        await syncHubClient.GameStarting(message.GameId, cancellationToken);
    }
}