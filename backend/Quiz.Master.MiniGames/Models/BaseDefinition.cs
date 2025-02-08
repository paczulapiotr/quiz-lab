using System.Text.Json.Serialization;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Master.MiniGames.Models.LettersAndPhrases;
using Quiz.Master.MiniGames.Models.MusicGuess;

namespace Quiz.Master.MiniGames.Models;

[JsonDerivedType(typeof(MusicGuessDefinition))]
[JsonDerivedType(typeof(AbcdWithCategoriesDefinition))]
[JsonDerivedType(typeof(LettersAndPhrasesDefinition))]
public record BaseDefinition : MiniGameDefinitionData
{
}