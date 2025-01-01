using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using Json.Schema;
using Quiz.Master.Persistance;
using Quiz.Master.Core.Models;

namespace Quiz.Master.Migrations;

public static class MigrationExtensions
{
    public static async Task MigrateDatabaseAsync(this WebApplication app, CancellationToken cancellationToken = default)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<QuizDbContext>();
            try
            {
                await dbContext.Database.MigrateAsync(cancellationToken);
                if (!await dbContext.MiniGameDefinitions.AnyAsync(cancellationToken))
                {
                    var jsonFilePath = Path.Combine(AppContext.BaseDirectory, "Migrations", "JsonDefinitions", "MiniGameDefinitions.json");
                    var jsonSchemaFilePath = Path.Combine(AppContext.BaseDirectory, "Migrations", "JsonDefinitions", "MiniGameDefinitions.Schema.json");

                    var jsonData = await File.ReadAllTextAsync(jsonFilePath, cancellationToken);
                    var jsonSchemaData = await File.ReadAllTextAsync(jsonSchemaFilePath, cancellationToken);

                    var schema = JsonSchema.FromText(jsonSchemaData);
                    var jsonArray = JsonNode.Parse(jsonData);

                    if (jsonArray is null)
                    {
                        throw new Exception("Json file is empty");
                    }

                    var validationResults = schema.Evaluate(jsonArray);
                    if (!validationResults.IsValid)
                    {
                        throw new Exception($"Validation failed: {string.Join(", ", validationResults.Errors?.Select(e => $"{e.Key}:{e.Value}") ?? Array.Empty<string>())}");
                    }

                    var miniGameDefinitions = new List<MiniGameDefinition>();

                    foreach (var jsonNode in jsonArray.AsArray())
                    {
                        var jsonObject = jsonNode!.AsObject();
                        var definitionJsonData = jsonObject["DefinitionJsonData"]!.ToJsonString();
                        jsonObject["DefinitionJsonData"] = definitionJsonData;

                        var miniGameDefinition = JsonSerializer.Deserialize<MiniGameDefinition>(jsonObject.ToJsonString());
                        if (miniGameDefinition != null)
                        {
                            miniGameDefinitions.Add(miniGameDefinition);
                        }
                    }

                    dbContext.MiniGameDefinitions.AddRange(miniGameDefinitions);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}