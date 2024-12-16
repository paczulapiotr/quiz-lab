namespace Quiz.Master.Persistance.Models.MiniGames;

public abstract record MiniGameDefinitionBase<TConfig>
where TConfig : class, new()
{
    public TConfig Config { get; set; } = new();
}






