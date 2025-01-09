namespace Quiz.Master.Core.Models;

public record MiniGameInstanceScore
{
    public Guid PlayerId { get; set; }
    public int Score { get; set; }

}

