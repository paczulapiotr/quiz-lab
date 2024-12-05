using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.JsonSerializer;
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
        IJsonSerializer jsonSerializer)
    : base(connection, queueDefinition, logger, jsonSerializer)
    {
        this.syncHubClient = syncHubClient;
    }

    protected override async Task ProcessMessageAsync(GameCreated message, CancellationToken cancellationToken = default)
    {
        await syncHubClient.GameCreated(new GameCreatedSyncMessage(message.GameId, message.GameSize), cancellationToken);
    }
}