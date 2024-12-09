namespace Quiz.Master.Persistance.Models;

public abstract record AnswerBase
{
    public required string Id { get; set; }
    public required string Text { get; set; }
    public bool IsCorrect { get; set; }
}
