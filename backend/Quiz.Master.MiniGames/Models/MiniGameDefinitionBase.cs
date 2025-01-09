using Quiz.Master.Core.Models;

namespace Quiz.Master.MiniGames.Models;


public abstract record MiniGameDefinitionBase<TConfig> : MiniGameDefinitionData
where TConfig : class, new()
{
    public TConfig Config { get; set; } = new();
}






