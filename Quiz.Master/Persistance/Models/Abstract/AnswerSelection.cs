namespace Quiz.Master.Persistance.Models;

public abstract record AnswerSelection<TAnswer> : IEntity where TAnswer : Answer
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Player Player { get; set; } = null!;
    public Guid AnswerId { get; set; }
    public TAnswer Answer { get; set; } = null!;
    public DateTime AnsweredAt { get; set; }
    public int Score { get; set; }
}
