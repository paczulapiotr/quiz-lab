using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;
using RabbitMQ.Client;

namespace Quiz.Common.Broker.Builder;

public class QueueConfig
{
    private readonly IServiceCollection _services;

    public QueueConfig(IServiceCollection services)
    {
        _services = services;
    }

    public QueueConfig AddConsumer<
    TMessage,
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TConsumer
    >(IQueueConsumerDefinition<TMessage> queueDefinition)
    where TConsumer : class, IConsumer<TMessage>
    where TMessage : IMessage

    {
        _services.AddSingleton<IConsumer, TConsumer>();
        _services.AddSingleton<IQueueConsumerDefinition>(queueDefinition);
        _services.AddSingleton(queueDefinition);
        return this;
    }

    public QueueConfig AddOneTimeConsumer<TMessage>(IQueueConsumerDefinition<TMessage> queueDefinition)
    where TMessage : class, IMessage
    {
        _services.AddSingleton<IOneTimeConsumer<TMessage>>(service
            => new OneTimeConsumer<TMessage>(
                service.GetRequiredService<IConnection>(),
                queueDefinition,
                service.GetRequiredService<ILogger<OneTimeConsumer<TMessage>>>()));
        _services.AddSingleton<IQueueConsumerDefinition>(queueDefinition);
        _services.AddSingleton(queueDefinition);

        return this;
    }

    public QueueConfig AddPublisher<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TMessage>(IQueuePublisherDefinition<TMessage> queueDefinition)
    where TMessage : IMessage
    {
        _services.AddSingleton<IQueuePublisherDefinition>(queueDefinition);
        _services.AddSingleton<IQueuePublisherDefinition<TMessage>>(queueDefinition);
        return this;
    }
}