namespace Quiz.Master.Core.Models;

public record SimpleMiniGameDefinition
{
    public Guid MiniGameDefinitionId { get; set; }
    public MiniGameType Type { get; set; }
}

