namespace Quiz.Master.Persistance.Models;

public record GameScore : IEntity
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Game Game { get; set; } = null!;
    public Guid PlayerId { get; set; }
    public Player Player { get; set; } = null!;
    public int ScoreValue { get; set; }
}