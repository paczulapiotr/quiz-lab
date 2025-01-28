namespace Quiz.Master.Core.Models;

public record MiniGameInstance : IEntity
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public List<MiniGameInstanceScore> PlayerScores { get; set; } = new List<MiniGameInstanceScore>();
    public Guid MiniGameDefinitionId { get; set; }
    public MiniGameType Type { get; set; }
    public MiniGameStateData State { get; set; } = new(); // property for mini game state rounds information
}

public record MiniGameStateData {
    public TState? As<TState>() where TState : MiniGameStateData, new() {
        if(this is TState state) {
            return state;
        } else {
            return null;
        }
    }
}