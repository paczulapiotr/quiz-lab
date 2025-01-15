using Quiz.Master.Core.Models;

namespace Quiz.Master.Services.ContentManagement;

public record ComponentSharedMinDefMusic : MiniGameDto
{
    public List<Round> Rounds { get; set; } = new();

    public override MiniGameType GameType => MiniGameType.Music;

    public record Round
    {
        public List<Answer> Answers { get; set; } = new();
        public required Audio Audio { get; set; }
    }

    override public MiniGameDefinitionData MapToDefinition()
    {
        throw new NotImplementedException();
    }
}