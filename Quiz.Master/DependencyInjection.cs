using Quiz.Master.Game;
using Quiz.Master.Game.Communication;
using Quiz.Master.Game.Repository;
using Quiz.Master.Game.Round;
using Quiz.Master.Persistance;

namespace Quiz.Master;

public static class DependencyInjection
{
    public static IServiceCollection AddQuizServices(this IServiceCollection services)
    {
        services.AddScoped<IQuizRepository, QuizSqlRepository>();
        services.AddScoped<IGameStateRepository, GameStateSqlRepository>();
        services.AddScoped<IGameRoundHandlerSelector, GameRoundHandlerSelector>();
        services.AddScoped<ICommunicationService, CommunicationService>();
        services.AddScoped<IGameEngine, GameEngine>();
        return services;
    }
}