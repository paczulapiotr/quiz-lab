using MongoDB.Bson.Serialization;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Master.MiniGames.Models.LettersAndPhrases;
using Quiz.Master.MiniGames.Models.MusicGuess;

public class MiniGameStateDataClassMap : BsonClassMap<MiniGameStateData>
{
    public MiniGameStateDataClassMap()
    {
        AutoMap();
        AddKnownType(typeof(AbcdWithCategoriesState));
        AddKnownType(typeof(MusicGuessState));
        AddKnownType(typeof(LettersAndPhrasesState));
    }
}