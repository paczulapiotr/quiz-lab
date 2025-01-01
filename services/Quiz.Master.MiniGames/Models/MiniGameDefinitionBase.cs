namespace Quiz.Master.MiniGames.Models;


public abstract record MiniGameDefinitionBase<TConfig>
where TConfig : class, new()
{
    public TConfig Config { get; set; } = new();
}






