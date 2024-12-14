using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.JsonSerializer;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages.Game;
using Quiz.Slave.Hubs;
using Quiz.Slave.Hubs.Models;
using RabbitMQ.Client;

namespace Quiz.Slave.Consumers;

internal class MiniGameUpdateConsumer : ConsumerBase<MiniGameUpdate>
{
    private readonly ISyncHubClient syncHubClient;

    public MiniGameUpdateConsumer(
        IConnection connection,
        ISyncHubClient syncHubClient,
        IQueueConsumerDefinition<MiniGameUpdate> queueDefinition,
        ILogger<MiniGameUpdateConsumer> logger,
        IJsonSerializer jsonSerializer)
    : base(connection, queueDefinition, logger, jsonSerializer)
    {
        this.syncHubClient = syncHubClient;
    }

    protected override async Task ProcessMessageAsync(MiniGameUpdate message, CancellationToken cancellationToken = default)
    {
        await syncHubClient.MiniGameUpdated(new MiniGameUpdateSyncMessage(message.GameId, message.MiniGameType, message.Status, message.Data), cancellationToken);
    }
}