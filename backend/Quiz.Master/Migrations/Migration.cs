using System.Text.Json;
using MongoDB.Driver;
using Quiz.Master.Core.Models;

namespace Quiz.Master.Migrations;
[MongoDB.Bson.Serialization.Attributes.BsonKnownTypes()]
public static class Migration
{
    public static void Run(IServiceProvider serviceProvider)
    {
        var database = serviceProvider.GetRequiredService<IMongoDatabase>();
        var gameDefinitionCollection = database.GetCollection<GameDefinition>("GameDefinitions");
        var miniGameDefinitionCollection = database.GetCollection<MiniGameDefinition>("MiniGameDefinitions");
        var jsonFilePath = "./Migrations/GameDefinitionMigrations.json";
        var jsonData = File.ReadAllText(jsonFilePath);

        // Deserialize JSON using Newtonsoft.Json
        var opts = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new MiniGameDefinitionConverter() }
        };
        var documents = JsonSerializer.Deserialize<List<GameDefinitionMigrationDto>>(jsonData, opts);

        if (documents != null)
        {
            foreach (var document in documents)
            {
                var existingDocument = gameDefinitionCollection.Find(x => x.Id == document.Id).Any();

                if (!existingDocument)
                {
                    // Convert the MiniGames to BsonDocument
                    miniGameDefinitionCollection.InsertMany(document.MiniGames);

                    gameDefinitionCollection.InsertOne(new GameDefinition
                    {
                        Id = document.Id,
                        CreatedAt = DateTime.UtcNow,
                        Name = document.Name,
                        MiniGames = document.MiniGames.Select(x => new SimpleMiniGameDefinition
                        {
                            MiniGameDefinitionId = x.Id,
                            Type = x.Type,
                        }).ToList()
                    });
                }
            }
        }
    }
}