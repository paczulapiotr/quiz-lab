namespace Quiz.Master.MiniGames.Models.Sorter;

public record Configuration
{
    public int TimeForAnswerMs { get; set; }
    public int PointsForCorrectAnswer { get; set; }
}