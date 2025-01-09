namespace Quiz.Master.Core.Models;

public record MiniGameDefinition<TDefinition> : IEntity where TDefinition : MiniGameDefinitionData, new()
{
    public Guid Id { get; set; }
    public required string Description { get; set; }
    public MiniGameType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ArchivedAt { get; set; }
    public bool Archived { get; set; }
    public TDefinition Definition { get; set; } = new(); // property for storing rounds definition information

}

public record MiniGameDefinitionData {
}
