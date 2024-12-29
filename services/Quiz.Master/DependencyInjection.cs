using Quiz.Master.Game;
using Quiz.Master.Game.Communication;
using Quiz.Master.Game.Repository;
using Quiz.Master.Game.MiniGames;
using Quiz.Master.Persistance;
using Quiz.Master.Persistance.Repositories;
using Quiz.Master.Persistance.Repositories.Abstract;
using Quiz.Common.CQRS;

namespace Quiz.Master;

public static class DependencyInjection
{
    public static IServiceCollection AddQuizServices(this IServiceCollection services)
    {
        services.AddScoped<IQuizRepository, QuizSqlRepository>();
        services.AddScoped<IGameStateRepository, GameStateSqlRepository>();
        services.AddScoped<IMiniGameHandlerSelector, MiniGameHandlerSelector>();
        services.AddScoped<ICommunicationService, CommunicationService>();
        services.AddScoped<AbcdWithCategoriesMiniGame>();
        services.AddScoped<IGameEngine, GameEngine>();
        services.AddScoped<IMiniGameSaveRepository, SqlMiniGameSaveRepository>();
        services.AddHandlers();

        return services;
    }

    private static void AddHandlers(this IServiceCollection services)
    {
        var commandHandlerType = typeof(ICommandHandler<>);
        var queryHandlerType = typeof(IQueryHandler<,>);

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    (i.GetGenericTypeDefinition() == commandHandlerType || i.GetGenericTypeDefinition() == queryHandlerType)))
                .ToList();

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();

                foreach (var @interface in interfaces)
                {
                    var genericTypeDefinition = @interface.GetGenericTypeDefinition();

                    if (genericTypeDefinition == commandHandlerType || genericTypeDefinition == queryHandlerType)
                    {
                        services.AddScoped(@interface, type);
                    }
                }
            }
        }
    }
}