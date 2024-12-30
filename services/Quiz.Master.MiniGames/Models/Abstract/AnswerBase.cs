namespace Quiz.Master.MiniGames.Models.Abstract;

public abstract record AnswerBase
{
    public required string Id { get; set; }
    public required string Text { get; set; }
    public bool IsCorrect { get; set; }
}
