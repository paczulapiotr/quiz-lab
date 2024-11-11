using Microsoft.Extensions.DependencyInjection;

namespace Quiz.CommonLib.MessageBroker.Builder;

public interface IMessageBrokerBuilder
{
    IServiceCollection Services { get; }
}