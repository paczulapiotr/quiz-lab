namespace Quiz.Master.Persistance.Models.MiniGames;

public abstract record MiniGameDataBase<TConfig, TState>
where TConfig : class, new()
where TState : class, new()
{
    public TConfig Config { get; set; } = new();
    public TState State { get; set; } = new();
}






