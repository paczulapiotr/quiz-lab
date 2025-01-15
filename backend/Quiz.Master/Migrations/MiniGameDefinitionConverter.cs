using System.Text.Json;
using System.Text.Json.Serialization;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.AbcdCategories;

public class MiniGameDefinitionConverter : JsonConverter<MiniGameDefinition>
{
    public override MiniGameDefinition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;

            var type = (MiniGameType)root.GetProperty("Type").GetInt32();
            var definitionElement = root.GetProperty("Definition");

            MiniGameDefinitionData? definition = type switch
            {
                MiniGameType.AbcdWithCategories => JsonSerializer.Deserialize<AbcdWithCategoriesDefinition>(definitionElement.GetRawText(), options),
                _ => JsonSerializer.Deserialize<MiniGameDefinitionData>(definitionElement.GetRawText(), options),
            };

            return new MiniGameDefinition
            {
                Id = root.GetProperty(nameof(MiniGameDefinition.Id)).GetGuid(),
                Type = type,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                ArchivedAt = null,
                Archived = false,
                Definition = definition ?? new()
            };
        }
    }

    public override void Write(Utf8JsonWriter writer, MiniGameDefinition value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, options);
    }
}