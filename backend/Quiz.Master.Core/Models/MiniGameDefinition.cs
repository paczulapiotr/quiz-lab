namespace Quiz.Master.Core.Models;

public record MiniGameDefinition : IEntity
{
    public Guid Id { get; set; }
    public MiniGameType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public MiniGameDefinitionData Definition { get; set; } = new(); // property for storing rounds definition information
}

public record MiniGameDefinitionData {
    public TDefinition? As<TDefinition>() where TDefinition : MiniGameDefinitionData, new()
    {
        if (this is TDefinition state)
        {
            return state;
        }
        else
        {
            return null;
        }
    }
}
