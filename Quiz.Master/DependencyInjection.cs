using Quiz.Master.Persistance;

namespace Quiz.Master;

public static class DependencyInjection
{
    public static IServiceCollection AddQuizServices(this IServiceCollection services)
    {
        services.AddScoped<IQuizRepository, QuizSqlRepository>();

        return services;
    }
}