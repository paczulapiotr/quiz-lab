using NUnit.Framework.Legacy;
using Quiz.Master.MiniGames.Handlers.FamilyFeud.Logic;
using Quiz.Master.MiniGames.Models.FamilyFeud;

namespace Quiz.Master.MiniGames.Tests;
[TestFixture]
public class AnswerMatch_Tests
{
    [Test]
    public void MatchAnswer_ReturnsMatch_WhenSynonymMatchesAnswer()
    {
        // Arrange
        var answers = new List<FamilyFeudDefinition.QuestionAnswer>
            {
                new FamilyFeudDefinition.QuestionAnswer
                {
                    Id = "1",
                    Synonyms = new List<string> { "tree", "flower" },
                    Answer = "plant",
                    Points = 10
                },
                new FamilyFeudDefinition.QuestionAnswer
                {
                    Id = "2",
                    Synonyms = new List<string> { "banana", "apple" },
                    Answer = "fruit",
                    Points = 10
                }
            };

        var usedAnswers = new List<string>();
        var inputAnswer = "appple"; // intentionally misspelled to test fuzzy match

        // Act
        var result = AnswerMatch.MatchAnswer(answers, inputAnswer, usedAnswers);

        // Assert
        ClassicAssert.NotNull(result, "Expected a matching answer result.");
        Assert.That(result?.answer.Id, Is.EqualTo("2"));
    }

    [Test]
    public void MatchAnswer_ReturnsNull_WhenNoValidMatchFound()
    {
        // Arrange
        var answers = new List<FamilyFeudDefinition.QuestionAnswer>
            {
                new FamilyFeudDefinition.QuestionAnswer
                {
                    Id = "1",
                    Synonyms = new List<string> { "apple", "fruit" },
                    Answer = "apple",
                    Points = 10
                },
            };
        var usedAnswers = new List<string> { "1" };
        var inputAnswer = "orange";

        // Act
        var result = AnswerMatch.MatchAnswer(answers, inputAnswer, usedAnswers);

        // Assert
        ClassicAssert.IsNull(result, "Expected no match if answer has been used or doesn't match.");
    }
}
