using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Quiz.Storage;

public static class Extensions
{
    public static void AddStorage(this IServiceCollection services, string connectionString, string databaseName)
    {
        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        });
        services.AddSingleton<IDatabaseStorage, DatabaseStorage>();
        DatabaseStorage.RegisterClassMap();
    }
}