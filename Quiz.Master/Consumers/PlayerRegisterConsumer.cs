using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages;
using Quiz.Master.Persistance;
using Quiz.Master.Persistance.Models;
using RabbitMQ.Client;

namespace Quiz.Master.Consumers
{
    public class PlayerRegisterConsumer : ConsumerBase<PlayerRegister>
    {
        private readonly IPublisher publisher;
        private readonly IDbContextFactory<QuizDbContext> dbContextFactory;

        public PlayerRegisterConsumer(
            IConnection connection,
            IQueueConsumerDefinition<PlayerRegister> queueDefinition,
            ILogger<PlayerRegisterConsumer> logger,
            JsonSerializerContext jsonSerializerContext,
            IPublisher publisher,
            IDbContextFactory<QuizDbContext> dbContextFactory)
        : base(connection, queueDefinition, logger, jsonSerializerContext)
        {
            this.publisher = publisher;
            this.dbContextFactory = dbContextFactory;
        }

        protected override async Task ProcessMessageAsync(PlayerRegister message, CancellationToken cancellationToken = default)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var game = new Game();
            await dbContext.Games.AddAsync(game, cancellationToken);
            await dbContext.Players.AddAsync(new Player
            {
                Name = message.PlayerName,
                DeviceId = message.UniqueId,
                Game = game,
            }, cancellationToken);

            await dbContext.SaveChangesAsync();

            await publisher.PublishAsync(new PlayerRegistered(message.UniqueId, message.PlayerName, message.CorrelationId), cancellationToken);
        }
    }
}