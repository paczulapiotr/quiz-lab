using System.Text.Json.Serialization;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages.Game;
using Quiz.Slave.Hubs;
using Quiz.Slave.Hubs.Models;
using RabbitMQ.Client;

namespace Quiz.Slave.Consumers;

internal class GameCreatedConsumer : ConsumerBase<GameCreated>
{
    private readonly ISyncHubClient syncHubClient;

    public GameCreatedConsumer(
        IConnection connection,
        ISyncHubClient syncHubClient,
        IQueueConsumerDefinition<GameCreated> queueDefinition,
        ILogger<GameCreatedConsumer> logger,
        JsonSerializerContext jsonSerializerContext)
    : base(connection, queueDefinition, logger, jsonSerializerContext)
    {
        this.syncHubClient = syncHubClient;
    }

    protected override async Task ProcessMessageAsync(GameCreated message, CancellationToken cancellationToken = default)
    {
        await syncHubClient.GameCreated(new GameCreatedSyncMessage(message.GameId, message.GameSize), cancellationToken);
    }
}