namespace Quiz.Master.Core.Models;

public record Game : IEntity
{
    public Guid Id { get; set; }
    public GameStatus Status { get; set; }
    public IEnumerable<MiniGameInstance> MiniGames { get; set; } = new List<MiniGameInstance>();
    public MiniGameInstance? CurrentMiniGame { get; set; }
    public Guid? CurrentMiniGameId { get; set; }
    public uint GameSize { get; set; }
    public ICollection<Player> Players { get; set; } = new List<Player>();
    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }

    public bool IsFinished => FinishedAt.HasValue;
    public bool IsStarted => StartedAt.HasValue;
    public int PlayersCount => Players.Count;
}

