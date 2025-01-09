
namespace Quiz.Master.Core.Models;

public record MiniGameInstance<TState> : IEntity where TState : MiniGameStateData, new()
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public List<MiniGameInstanceScore> PlayerScores { get; set; } = new List<MiniGameInstanceScore>();
    public Guid MiniGameDefinitionId { get; set; }
    public MiniGameType Type { get; set; }
    public TState State { get; set; } = new(); // property for mini game state rounds information
}

public record MiniGameStateData {}