namespace Quiz.Master.MiniGames.Models.FamilyFeud;

public record FamilyFeudDefinition : BaseDefinition
{
    public List<Round> Rounds { get; set; } = new List<Round>();

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
        public required IEnumerable<string> Synonyms { get; set; }
        public required int Points { get; set; }
    }
}






