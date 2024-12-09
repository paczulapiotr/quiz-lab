namespace Quiz.Master.Persistance.Models;

public abstract record Question<TAnswer> : IEntity where TAnswer : Answer
{
    public Guid Id { get; set; }
    public required string Content { get; set; }
    public required Guid CorrectAnswerId { get; set; }
    public required TAnswer CorrectAnswer { get; set; }
    public ICollection<TAnswer> Answers { get; set; } = new List<TAnswer>();
    public DateTime CreatedAt { get; set; }
}
