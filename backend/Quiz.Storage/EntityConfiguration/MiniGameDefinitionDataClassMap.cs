using MongoDB.Bson.Serialization;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Master.MiniGames.Models.LettersAndPhrases;
using Quiz.Master.MiniGames.Models.MusicGuess;
using Quiz.Master.MiniGames.Models.Sorter;

public class MiniGameDefinitionDataClassMap : BsonClassMap<MiniGameDefinitionData>
{
    public MiniGameDefinitionDataClassMap()
    {
        AutoMap();
        AddKnownType(typeof(AbcdWithCategoriesDefinition));
        AddKnownType(typeof(MusicGuessDefinition));
        AddKnownType(typeof(LettersAndPhrasesDefinition));
        AddKnownType(typeof(SorterDefinition));
    }
}