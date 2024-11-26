using System.Text.Json.Serialization;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages;
using Quiz.Slave.Hubs;
using Quiz.Slave.Hubs.Models;
using RabbitMQ.Client;

namespace Quiz.Slave.Consumers;

internal class PlayerJoinedConsumer : ConsumerBase<PlayerJoined>
{
    private readonly ISyncHubClient syncHubClient;

    public PlayerJoinedConsumer(
        IConnection connection,
        ISyncHubClient syncHubClient,
        IQueueConsumerDefinition<PlayerJoined> queueDefinition,
        ILogger<PlayerJoinedConsumer> logger,
        JsonSerializerContext jsonSerializerContext)
    : base(connection, queueDefinition, logger, jsonSerializerContext)
    {
        this.syncHubClient = syncHubClient;
    }

    protected override async Task ProcessMessageAsync(PlayerJoined message, CancellationToken cancellationToken = default)
    {
        await syncHubClient.PlayerJoined(new PlayerJoinedSyncMessage(message.DeviceId, message.PlayerName), cancellationToken);
    }
}