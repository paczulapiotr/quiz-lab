namespace Quiz.Master.Persistance.Models;

public record Question : IEntity
{
    public Guid Id { get; set; }
    public required string Content { get; set; }
    public Guid GameId { get; set; }
    public required Game Game { get; set; }
    public required Guid CorrectAnswerId { get; set; }
    public required Answer CorrectAnswer { get; set; }
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    public DateTime CreatedAt { get; set; }
}
