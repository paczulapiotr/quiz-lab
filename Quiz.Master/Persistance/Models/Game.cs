using Quiz.Common.Messages.Game;
using Quiz.Master.Persistance.Models.MiniGames.AbcdCategories;

namespace Quiz.Master.Persistance.Models;

public record Game : IEntity
{
    public Guid Id { get; set; }
    public GameStatus Status { get; set; }
    public IEnumerable<MiniGame> MiniGames { get; set; } = new List<MiniGame>();
    public MiniGameType? CurrentMiniGame { get; set; }
    public uint GameSize { get; set; }
    public ICollection<Player> Players { get; set; } = new List<Player>();
    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }

    public bool IsFinished => FinishedAt.HasValue;
    public bool IsStarted => StartedAt.HasValue;

    // Mini games
    // ...
}

