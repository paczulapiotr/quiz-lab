namespace Quiz.Master.Persistance.Models;

public record Answer
{
    public Guid Id { get; set; }
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public required string Content { get; set; }
    public bool IsCorrect { get; set; }
}
