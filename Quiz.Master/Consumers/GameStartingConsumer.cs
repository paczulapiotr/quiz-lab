using System.Text.Json.Serialization;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Broker.QueueDefinitions;
using Quiz.Common.Messages;
using Quiz.Master.Persistance;
using RabbitMQ.Client;

namespace Quiz.Master.Consumers;

internal class GameStartingConsumer : ConsumerBase<GameStarting>
{
    private readonly IPublisher publisher;
    private readonly IServiceProvider serviceProvider;

    public GameStartingConsumer(
        IConnection connection,
        IQueueConsumerDefinition<GameStarting> queueDefinition,
        ILogger<GameStartingConsumer> logger,
        JsonSerializerContext jsonSerializerContext,
        IPublisher publisher,
        IServiceProvider serviceProvider)
    : base(connection, queueDefinition, logger, jsonSerializerContext)
    {
        this.publisher = publisher;
        this.serviceProvider = serviceProvider;
    }

    protected override async Task ProcessMessageAsync(GameStarting message, CancellationToken cancellationToken = default)
    {
        var delay = Task.Delay(61_000);

        using (var scope = serviceProvider.CreateScope())
        {
            var quizRepository = scope.ServiceProvider.GetRequiredService<IQuizRepository>();
            var game = await quizRepository.GetAsync<Persistance.Models.Game>(Guid.Parse(message.GameId));
            game!.StartedAt = DateTime.UtcNow;
            await quizRepository.SaveChangesAsync(cancellationToken);
        }

        await delay;
        await publisher.PublishAsync(new GameStarted(message.GameId), cancellationToken);
    }
}