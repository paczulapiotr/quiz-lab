using FuzzySharp;
using Quiz.Master.MiniGames.Models.FamilyFeud;

namespace Quiz.Master.MiniGames.Handlers.FamilyFeud.Logic;

public static class AnswerMatch
{
    public static (FamilyFeudDefinition.QuestionAnswer answer, string synonym)? MatchAnswer(IEnumerable<FamilyFeudDefinition.QuestionAnswer> answers, string answer, IEnumerable<string> usedAnswers)
    {
        var match = answers.Where(x => !usedAnswers.Contains(x.Id))
            .Select(x
                => x.Synonyms
                    .Select(s => new
                    {
                        Ratio = Fuzz.Ratio(s, answer),
                        Answer = x,
                        Synonym = s
                    })
                    .OrderByDescending(x => x.Ratio).FirstOrDefault())
            .Where(x => x != null)
            .Where(x => (x!.Synonym.Length - 1) / x!.Synonym.Length <= x.Ratio)
            .FirstOrDefault();

        if (match == null) return null;

        return (match.Answer, match.Synonym);
    }
}