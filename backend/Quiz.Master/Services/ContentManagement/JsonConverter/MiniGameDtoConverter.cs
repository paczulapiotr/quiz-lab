using System.Text.Json;
using System.Text.Json.Serialization;

namespace Quiz.Master.Services.ContentManagement;

public class MiniGameDtoConverter : JsonConverter<MiniGameDto>
{
    public override MiniGameDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;

            if (root.TryGetProperty("__typename", out JsonElement typeNameElement))
            {
                var typeName = typeNameElement.GetString();
                switch (typeName)
                {
                    case "ComponentSharedMiniGameDefAbcd":
                        return JsonSerializer.Deserialize<ComponentSharedMiniGameDefAbcd>(root.GetRawText(), options)!;
                    case "ComponentSharedMinDefMusic":
                        return JsonSerializer.Deserialize<ComponentSharedMinDefMusic>(root.GetRawText(), options)!;
                    default:
                        throw new JsonException($"Unknown mini-game type: {typeName}");
                }
            }
            else
            {
                throw new JsonException("Missing __typename property");
            }
        }
    }

    public override void Write(Utf8JsonWriter writer, MiniGameDto value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, options);
    }
}