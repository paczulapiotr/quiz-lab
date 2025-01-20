namespace Quiz.Master.MiniGames.Models.MusicGuess;

public record Configuration
{
    public int TimeForCategorySelectionMs { get; set; }
    public int TimeForAnswerSelectionMs { get; set; }
    public int MaxPointsForAnswer { get; set; }
    public int MinPointsForAnswer { get; set; }
    public int PointsDecrement { get; set; }
}