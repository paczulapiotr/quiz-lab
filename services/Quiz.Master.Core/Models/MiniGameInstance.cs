
namespace Quiz.Master.Core.Models;

public record MiniGameInstance : IEntity
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Game Game { get; set; } = null!;
    public List<MiniGameInstanceScore> PlayerScores { get; set; } = new List<MiniGameInstanceScore>();
    public Guid MiniGameDefinitionId { get; set; }
    public MiniGameDefinition MiniGameDefinition { get; set; } = null!;
    public string StateJsonData { get; set; } = string.Empty; // JSON property for mini game state rounds information
}
