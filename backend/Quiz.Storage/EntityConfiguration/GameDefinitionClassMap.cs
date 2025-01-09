using MongoDB.Bson.Serialization;
using Quiz.Master.Core.Models;

public class GameDefinitionClassMap : BsonClassMap<GameDefinition>
{
    public GameDefinitionClassMap()
    {
        AutoMap();
        MapIdMember(c => c.Id);
        MapMember(c => c.CreatedAt).SetDefaultValue(DateTime.UtcNow);
    }
}