namespace Quiz.Master.Core.Models;

public record SimpleMiniGame
{
    public Guid MiniGameId { get; set; }
    public Guid MiniGameDefinitionId { get; set; }
    public MiniGameType Type { get; set; }
}

