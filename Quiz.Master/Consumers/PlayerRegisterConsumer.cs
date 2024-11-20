using System.Text.Json.Serialization;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages;
using RabbitMQ.Client;

namespace Quiz.Master.Consumers
{
    public class PlayerRegisterConsumer : ConsumerBase<PlayerRegister>
    {
        private readonly IPublisher publisher;

        public PlayerRegisterConsumer(
            IConnection connection,
            IQueueConsumerDefinition<PlayerRegister> queueDefinition,
            ILogger<PlayerRegisterConsumer> logger,
            JsonSerializerContext jsonSerializerContext,
            IPublisher publisher)
        : base(connection, queueDefinition, logger, jsonSerializerContext)
        {
            this.publisher = publisher;
        }

        protected override async Task ProcessMessageAsync(PlayerRegister message, CancellationToken cancellationToken = default)
        {
            await publisher.PublishAsync(new PlayerRegistered(message.UniqueId, message.PlayerName, message.CorrelationId), cancellationToken);
        }
    }
}