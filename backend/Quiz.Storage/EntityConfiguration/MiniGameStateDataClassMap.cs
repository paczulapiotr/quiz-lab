using MongoDB.Bson.Serialization;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.AbcdCategories;

public class MiniGameStateDataClassMap : BsonClassMap<MiniGameStateData>
{
    public MiniGameStateDataClassMap()
    {
        AutoMap();
        AddKnownType(typeof(AbcdWithCategoriesState));
    }
}