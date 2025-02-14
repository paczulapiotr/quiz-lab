using Quiz.Master.Game;
using Quiz.Master.Game.Communication;
using Quiz.Master.Game.MiniGames;
using Quiz.Master.Persistance.Repositories;
using Quiz.Master.Persistance.Repositories.Abstract;
using Quiz.Common.CQRS;
using IAbcdWithCategoriesEventService = Quiz.Master.MiniGames.Handlers.AbcdWithCategories.IMiniGameEventService;
using AbcdWithCategoriesEventService = Quiz.Master.Game.MiniGames.AbcdWithCategories.MiniGameEventService;
using IMusicGuessEventService = Quiz.Master.MiniGames.Handlers.MusicGuess.IMiniGameEventService;
using MiniGameEventService = Quiz.Master.Game.MiniGames.MusicGuess.MiniGameEventService;
using ILettersAndPhrasesEventService = Quiz.Master.MiniGames.Handlers.LettersAndPhrases.IMiniGameEventService;
using LettersAndPhrasesEventService = Quiz.Master.Game.MiniGames.LettersAndPhrases.MiniGameEventService;
using ISorterEventService = Quiz.Master.MiniGames.Handlers.Sorter.IMiniGameEventService;
using SorterEventService = Quiz.Master.Game.MiniGames.Sorter.MiniGameEventService;
using Quiz.Master.Services.Lights;
using Quiz.Master.Services.ContentManagement;
using AbcdConfiguration = Quiz.Master.MiniGames.Models.AbcdCategories.Configuration;
using MusicConfiguration = Quiz.Master.MiniGames.Models.MusicGuess.Configuration;
using LettersConfiguration = Quiz.Master.MiniGames.Models.LettersAndPhrases.Configuration;
using SorterConfiguration = Quiz.Master.MiniGames.Models.Sorter.Configuration;
using Quiz.Master.MiniGames.Handlers.AbcdWithCategories;
using Quiz.Master.MiniGames.Handlers.MusicGuess;
using Quiz.Master.MiniGames.Handlers.LettersAndPhrases;
using Quiz.Master.MiniGames.Handlers.Sorter;

namespace Quiz.Master;

public static class DependencyInjection
{
    public static IServiceCollection AddQuizServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.Configure<LightsConfig>(configuration.GetSection("Lights"));
        services.AddTransient<ILightsClient, LightsClient>();
        services.AddTransient<IContentManagementClient, ContentManagementClient>();
        services.AddTransient<IGameRepository, StorageGameRepository>();
        services.AddTransient<IMiniGameRepository, StorageMiniGameRepository>();
        services.AddTransient<IMiniGameHandlerSelector, MiniGameHandlerSelector>();
        services.AddTransient<ICommunicationService, CommunicationService>();

        services.Configure<ContentManagementConfig>(configuration.GetSection("ContentManagement"));
        services.Configure<DefaultConfiguration>(configuration.GetSection("Game:Default"));
        services.Configure<AbcdConfiguration>(configuration.GetSection("Game:Abcd"));
        services.Configure<MusicConfiguration>(configuration.GetSection("Game:Music"));
        services.Configure<LettersConfiguration>(configuration.GetSection("Game:Letters"));
        services.Configure<SorterConfiguration>(configuration.GetSection("Game:Sorter"));

        // AbcdWithCategories
        services.AddTransient<IAbcdWithCategoriesEventService, AbcdWithCategoriesEventService>();
        services.AddTransient<AbcdWithCategoriesHandler>();

        // MusicGuess
        services.AddTransient<IMusicGuessEventService, MiniGameEventService>();
        services.AddTransient<MusicGuessHandler>();

        // LettersAndPhrases
        services.AddTransient<ILettersAndPhrasesEventService, LettersAndPhrasesEventService>();
        services.AddTransient<LettersAndPhrasesHandler>();

        // Sorter
        services.AddTransient<ISorterEventService, SorterEventService>();
        services.AddTransient<SorterHandler>();

        services.AddTransient<IGameEngine, GameEngine>();
        services.AddHandlers();

        return services;
    }

    private static void AddHandlers(this IServiceCollection services)
    {
        var commandHandlerType = typeof(ICommandHandler<>);
        var queryHandlerType = typeof(IQueryHandler<,>);
        var requestHandlerType = typeof(IRequestHandler<,>);

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    (i.GetGenericTypeDefinition() == commandHandlerType || i.GetGenericTypeDefinition() == queryHandlerType || i.GetGenericTypeDefinition() == requestHandlerType)))
                .ToList();

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();

                foreach (var @interface in interfaces)
                {
                    var genericTypeDefinition = @interface.GetGenericTypeDefinition();

                    if (genericTypeDefinition == commandHandlerType || genericTypeDefinition == queryHandlerType || genericTypeDefinition == requestHandlerType)
                    {
                        services.AddTransient(@interface, type);
                    }
                }
            }
        }
    }
}