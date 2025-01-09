using MongoDB.Bson.Serialization;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.AbcdCategories;

public class MiniGameDefinitionDataClassMap : BsonClassMap<MiniGameDefinitionData>
{
    public MiniGameDefinitionDataClassMap()
    {
        AutoMap();
        AddKnownType(typeof(AbcdWithCategoriesDefinition));
    }
}