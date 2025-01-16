namespace Quiz.Master.MiniGames.Models.AbcdCategories;

public record Configuration
{
    public int TimeForCategorySelectionMs { get; set; }
    public int TimeForPowerPlaySelectionMs { get; set; }
    public int TimeForAnswerSelectionMs { get; set; }
    public int MaxPointsForAnswer { get; set; }
    public int MinPointsForAnswer { get; set; }
    public int PointsDecrement { get; set; }
}