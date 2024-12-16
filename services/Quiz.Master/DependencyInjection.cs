using Quiz.Master.Game;
using Quiz.Master.Game.Communication;
using Quiz.Master.Game.Repository;
using Quiz.Master.Game.MiniGames;
using Quiz.Master.Persistance;
using Quiz.Master.Persistance.Repositories;
using Quiz.Master.Persistance.Repositories.Abstract;

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
        return services;
    }
}