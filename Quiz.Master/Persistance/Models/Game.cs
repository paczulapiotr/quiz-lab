namespace Quiz.Master.Persistance.Models;

public record Game
{
    public Guid Id { get; set; }
    public Guid GameScoreId { get; set; }
    public GameScore GameScore { get; set; } = null!;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<Player> Players { get; set; } = new List<Player>();
    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }

    public bool IsFinished => FinishedAt.HasValue;
    public bool IsStarted => StartedAt.HasValue;
}
