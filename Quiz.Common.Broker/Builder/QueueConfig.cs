using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Messages;
using Quiz.Common.Broker.QueueDefinitions;

namespace Quiz.Common.Broker.Builder;

public class QueueConfig
{
    private readonly IServiceCollection _services;

    public QueueConfig(IServiceCollection services)
    {
        _services = services;
    }

    public QueueConfig AddDefinition<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TMessage,
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TQueueDefinition>()
    where TQueueDefinition : class, IQueueDefinition<TMessage>, new()
    where TMessage : IMessage
    {
        _services.AddSingleton<IQueueDefinition<TMessage>, TQueueDefinition>();
        _services.AddSingleton<IQueueDefinition, TQueueDefinition>();
        return this;
    }

    public QueueConfig AddConsumer<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TConsumer>()
    where TConsumer : class, IConsumer
    {
        _services.AddSingleton<IConsumer, TConsumer>();
        return this;
    }
}