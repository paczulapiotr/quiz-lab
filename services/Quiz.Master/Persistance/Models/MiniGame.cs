
namespace Quiz.Master.Persistance.Models;

public record MiniGame : IEntity
{
    public Guid Id { get; set; }
    public MiniGameType Type { get; set; }
    public Guid GameId { get; set; }
    public Game Game { get; set; } = null!;
    public IEnumerable<MiniGameScore> PlayerScores { get; set; } = new List<MiniGameScore>();
}

public enum MiniGameType
{
    Democratic_ABCD,
    Democratic_ABCD_Quickest,
}
