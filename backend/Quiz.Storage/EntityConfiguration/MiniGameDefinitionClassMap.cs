using MongoDB.Bson.Serialization;
using Quiz.Master.Core.Models;

public class MiniGameDefinitionClassMap : BsonClassMap<MiniGameDefinition>
{
    public MiniGameDefinitionClassMap()
    {
        AutoMap();
        MapIdMember(c => c.Id);
    }
}

