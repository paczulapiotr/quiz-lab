namespace Quiz.Master.MiniGames.Models.LettersAndPhrases;

public record LettersAndPhrasesDefinition : BaseDefinition
{
    public List<Round> Rounds { get; set; } = new List<Round>();

    public record Round
    {
        public required string Id { get; set; }
        public required string Phrase { get; set; }
    }
}






