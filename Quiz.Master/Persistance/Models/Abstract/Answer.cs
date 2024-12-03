namespace Quiz.Master.Persistance.Models;

public abstract record Answer : IEntity
{
    public Guid Id { get; set; }
    public required string Content { get; set; }
    public bool IsCorrect { get; set; }
}
