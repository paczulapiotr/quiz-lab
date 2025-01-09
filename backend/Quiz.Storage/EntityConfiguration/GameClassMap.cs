using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Quiz.Master.Core.Models;

public class GameClassMap : BsonClassMap<Game>
{
    public GameClassMap()
    {
        AutoMap();
        MapIdMember(c => c.Id);
        MapMember(c => c.CreatedAt).SetDefaultValue(DateTime.UtcNow);
    }
}