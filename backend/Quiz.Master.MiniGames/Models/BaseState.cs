using System.Text.Json.Serialization;
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Master.MiniGames.Models.FamilyFeud;
using Quiz.Master.MiniGames.Models.LettersAndPhrases;
using Quiz.Master.MiniGames.Models.MusicGuess;
using Quiz.Master.MiniGames.Models.Sorter;

namespace Quiz.Master.MiniGames.Models;

[JsonDerivedType(typeof(MusicGuessState))]
[JsonDerivedType(typeof(AbcdWithCategoriesState))]
[JsonDerivedType(typeof(LettersAndPhrasesState))]
[JsonDerivedType(typeof(SorterState))]
[JsonDerivedType(typeof(FamilyFeudState))]
public record BaseState : MiniGameStateData
{
}