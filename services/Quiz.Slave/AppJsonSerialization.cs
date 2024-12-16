using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Quiz.Common.Broker.JsonSerializer;
using Quiz.Common.Messages.Game;
using Quiz.Slave.ApiModels.Ping;
using Quiz.Slave.Hubs.Models;

namespace Quiz.Slave;

// HTTP REST messages
[JsonSerializable(typeof(PingRequest))]

// Message Broker messages
[JsonSerializable(typeof(GameStatusUpdate))]
[JsonSerializable(typeof(MiniGameNotification))]

// Hub messages
[JsonSerializable(typeof(SelectAnswer))]
[JsonSerializable(typeof(GameCreatedSyncMessage))]
[JsonSerializable(typeof(PlayerJoinedSyncMessage))]
[JsonSerializable(typeof(GameStatusUpdateSyncMessage))]
[JsonSerializable(typeof(MiniGameNotificationSyncMessage))]

internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}

internal class AppJsonSerialization(JsonSerializerOptions jsonSerializerOptions) : IJsonSerializer
{
    public T? Deserialize<T>(string json)
    {
        var typeInfo = jsonSerializerOptions.GetTypeInfo(typeof(T)) as JsonTypeInfo<T>;
        return JsonSerializer.Deserialize(json, typeInfo!);
    }

    public string Serialize<T>(T obj)
    {
        var typeInfo = jsonSerializerOptions.GetTypeInfo(typeof(T)) as JsonTypeInfo<T>;
        return JsonSerializer.Serialize(obj, typeInfo!);
    }
}