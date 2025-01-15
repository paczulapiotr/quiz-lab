using MongoDB.Bson;
using Quiz.Master.Core.Models;

namespace Quiz.Master.Migrations; 

public record GameDefinitionMigrationDto {
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IEnumerable<MiniGameDefinition> MiniGames { get; set; } = new List<MiniGameDefinition> ();
}