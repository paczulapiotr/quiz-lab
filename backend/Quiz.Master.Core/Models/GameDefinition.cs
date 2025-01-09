namespace Quiz.Master.Core.Models;

public record GameDefinition : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IEnumerable<SimpleMiniGameDefinition> MiniGames { get; set; } = new List<SimpleMiniGameDefinition>();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

