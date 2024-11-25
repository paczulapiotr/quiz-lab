namespace Quiz.Master.Persistance.Models;

public record AnswerSelection : IEntity
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Player Player { get; set; } = null!;
    public Guid AnswerId { get; set; }
    public Answer Answer { get; set; } = null!;
    public DateTime AnsweredAt { get; set; }
    public int Score { get; set; }
}
