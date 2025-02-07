using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.LettersAndPhrases;

namespace Quiz.Master.Services.ContentManagement;

public record ComponentSharedMinDefLetters : MiniGameDto
{
    public List<Round> Rounds { get; set; } = new();

    public override MiniGameType GameType => MiniGameType.LettersAndPhrases;

    public record Round
    {
        public required string Id { get; set; }
        public required string Phrase { get; set; }

    }
    override public MiniGameDefinitionData MapToDefinition()
    {
        return new LettersAndPhrasesDefinition
        {
            Rounds = Rounds.Select(r => new LettersAndPhrasesDefinition.Round
            {
                Id = r.Id,
                Phrase = r.Phrase
            }).ToList()
        };
    }
}