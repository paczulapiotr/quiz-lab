namespace Quiz.Master.Services.ContentManagement;

public record Audio
{
    public required string Name { get; set; }
    public required string Ext { get; set; }
    public required string Url { get; set; }
}