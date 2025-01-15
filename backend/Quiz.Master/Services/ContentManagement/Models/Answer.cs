namespace Quiz.Master.Services.ContentManagement;

public record Answer
{
    public required string Id { get; set; }
    public bool IsCorrect { get; set; }
    public required string Text { get; set; }
}

