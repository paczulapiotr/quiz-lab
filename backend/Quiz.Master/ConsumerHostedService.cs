using Quiz.Common.Broker.Consumer;

namespace Quiz.Master;

public class ConsumerHostedService(
    ILogger<ConsumerHostedService> logger,
    IEnumerable<IConsumer> consumers
    ) : IHostedService
{

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting consumer hosted service");
        await Task.WhenAll(consumers.Select(c => c.ConsumeAsync()));
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var consumer in consumers)
        {
            await consumer.DisposeAsync();
        }
    }
}


