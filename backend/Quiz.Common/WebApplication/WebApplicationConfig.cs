using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Quiz.Common.CQRS;

namespace Quiz.Common.WebApplication;


public class WebApplicationConfig
{
    private readonly IServiceCollection _services;

    public WebApplicationConfig(IServiceCollection services)
    {
        _services = services;
    }

    public WebApplicationConfig AddQueryHandler<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TQueryHandlerInterface,
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TQueryHandlerImplementation,
    TQuery,
     TQueryResult
     >()
    where TQueryHandlerInterface : class, IQueryHandler<TQuery, TQueryResult>
    where TQueryHandlerImplementation : class, TQueryHandlerInterface
    where TQuery : IQuery<TQueryResult>
    {
        _services.AddScoped<TQueryHandlerInterface, TQueryHandlerImplementation>();
        if (typeof(IQueryHandler<TQuery, TQueryResult>) != typeof(TQueryHandlerInterface))
        {
            _services.AddScoped<IQueryHandler<TQuery, TQueryResult>, TQueryHandlerImplementation>();
        }
        return this;
    }

    public WebApplicationConfig AddQueryHandler<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TQueryHandlerImplementation,
    TQuery,
    TQueryResult
    >()
    where TQueryHandlerImplementation : class, IQueryHandler<TQuery, TQueryResult>
    where TQuery : IQuery<TQueryResult>
    {
        _services.AddScoped<IQueryHandler<TQuery, TQueryResult>, TQueryHandlerImplementation>();
        return this;
    }

    public WebApplicationConfig AddCommandHandler<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TCommandHandlerInterface,
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TCommandHandlerImplementation,
    TCommand
     >()
    where TCommandHandlerInterface : class, ICommandHandler<TCommand>
    where TCommandHandlerImplementation : class, TCommandHandlerInterface
    where TCommand : ICommand
    {
        _services.AddScoped<TCommandHandlerInterface, TCommandHandlerImplementation>();
        if (typeof(ICommandHandler<TCommand>) != typeof(TCommandHandlerInterface))
        {
            _services.AddScoped<ICommandHandler<TCommand>, TCommandHandlerImplementation>();
        }
        return this;
    }

    public WebApplicationConfig AddCommandHandler<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TCommandHandlerImplementation,
    TCommand
    >()
    where TCommandHandlerImplementation : class, ICommandHandler<TCommand>
    where TCommand : ICommand
    {
        _services.AddScoped<ICommandHandler<TCommand>, TCommandHandlerImplementation>();
        return this;
    }
}
