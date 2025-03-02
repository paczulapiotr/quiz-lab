using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.FamilyFeud;

namespace Quiz.Master.Services.ContentManagement;

public record ComponentSharedMinDefFamilyFeud : MiniGameDto
{
    public List<Round> Rounds { get; set; } = new();

    public override MiniGameType GameType => MiniGameType.FamilyFeud;

    public record Round
    {
        public required string Id { get; set; }
        public required string Question { get; set; }
        public required IEnumerable<QuestionAnswer> Answers { get; set; }
    }

    public record QuestionAnswer
    {
        public required string Id { get; set; }
        public required string Answer { get; set; }
        public required IEnumerable<Synonym> Synonyms { get; set; }
        public required int Points { get; set; }
    }

    public record Synonym
    {
        public required string Name { get; set; }
    }

    override public MiniGameDefinitionData MapToDefinition()
    {
        return new FamilyFeudDefinition
        {
            Rounds = Rounds.Select(r => new FamilyFeudDefinition.Round
            {
                Id = r.Id,
                Answers = r.Answers.Select(a => new FamilyFeudDefinition.QuestionAnswer
                {
                    Id = a.Id,
                    Answer = a.Answer,
                    Points = a.Points,
                    Synonyms = a.Synonyms?.Select(x=>x.Name) ?? [],
                }).ToList(),
                Question = r.Question
            }).ToList()
        };
    }
}