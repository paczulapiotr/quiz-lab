
namespace Quiz.Master.Persistance.Models;

public record MiniGameDefinition : IEntity
{
    public Guid Id { get; set; }
    public required string Description { get; set; }
    public MiniGameType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ArchivedAt { get; set; }
    public bool Archived { get; set; }
    public string DefinitionJsonData { get; set; } = string.Empty; // JSON property for storing rounds information

}