using System.Text.Json.Serialization;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages;
using RabbitMQ.Client;

namespace Quiz.Slave.Consumers
{
    public class PlayerRegisteredConsumer : ConsumerBase<PlayerRegistered>
    {

        public PlayerRegisteredConsumer(
            IConnection connection,
            IQueueConsumerDefinition<PlayerRegistered> queueDefinition,
            ILogger<PlayerRegisteredConsumer> logger,
            JsonSerializerContext jsonSerializerContext)
        : base(connection, queueDefinition, logger, jsonSerializerContext)
        {
        }

        protected override Task ProcessMessageAsync(PlayerRegistered message, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Player {PlayerName} with ID {UniqueId} has been registered", message.PlayerName, message.UniqueId);
            return Task.CompletedTask;
        }
    }
}