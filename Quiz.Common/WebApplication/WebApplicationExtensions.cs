using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Quiz.Common.CQRS;
using Quiz.Common.CQRS.Behaviors;
using Quiz.Common.Hubs;
using Quiz.Common.Middlewares;

namespace Quiz.Common.WebApplication;

public static class WebApplicationExtensions
{
    public static void AddQuizCommonServices(this IServiceCollection services, Action<WebApplicationConfig> configure)
    {
        // services.AddSingleton<>();
        services.AddHealthChecks();
        services.AddHttpContextAccessor();
        services.AddScoped<CorrelationIdMiddleware>();
        services.AddSingleton(typeof(IBehaviorPipeline<,>), typeof(LoggingBehavior<,>));

        var config = new WebApplicationConfig(services);
        configure(config);
    }

    public static void AddQuizHub<
    THub,
    ISyncHub,
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TSyncHub>
    (this IServiceCollection services)
    where THub : Hub
    where TSyncHub : SyncHubClientBase<THub>, ISyncHub
    where ISyncHub : class
    {
        services.AddSingleton<IHubConnection, HubConnection>();
        services.AddSingleton<ISyncHub, TSyncHub>();
    }

    public static void UseQuizCommonServices(this IApplicationBuilder app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseHealthChecks("/health");
    }
}