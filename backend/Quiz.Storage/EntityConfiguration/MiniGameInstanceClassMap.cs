using MongoDB.Bson.Serialization;
using Quiz.Master.Core.Models;

public class MiniGameInstanceClassMap : BsonClassMap<MiniGameInstance>
{
    public MiniGameInstanceClassMap()
    {
        AutoMap();
        MapIdMember(c => c.Id);
    }
}