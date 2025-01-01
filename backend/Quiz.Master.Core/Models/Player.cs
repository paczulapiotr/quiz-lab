namespace Quiz.Master.Core.Models;

public record Player : IEntity
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Game Game { get; set; } = null!;
    public required string Name { get; set; }
    public required string DeviceId { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<MiniGameInstanceScore> Scores { get; set; } = new List<MiniGameInstanceScore>();

}
