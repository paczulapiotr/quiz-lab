using FuzzySharp;
using Quiz.Master.MiniGames.Models.FamilyFeud;

namespace Quiz.Master.MiniGames.Handlers.FamilyFeud.Logic;

public static class AnswerMatch
{
    public static (FamilyFeudDefinition.QuestionAnswer answer, string synonym)? MatchAnswer(IEnumerable<FamilyFeudDefinition.QuestionAnswer> answers, string answer, IEnumerable<string> usedAnswers, int threshold = 80)
    {
        var match = answers.Where(x => !usedAnswers.Contains(x.Id))
            .Select(x
                => x.Synonyms
                    .Select(s => {
                        var ratio = Fuzz.Ratio(s.ToUpper(), answer.ToUpper());
                        return new
                        {
                            Ratio = ratio,
                            Answer = x,
                            Synonym = s
                        };
                    })
                    .OrderByDescending(x => x.Ratio).FirstOrDefault())
            .Where(x => x != null && x.Ratio >= threshold)
            .FirstOrDefault();

        if (match == null) return null;

        return (match.Answer, match.Synonym);
    }
}