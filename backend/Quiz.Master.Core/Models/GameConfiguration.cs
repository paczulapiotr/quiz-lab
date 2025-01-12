namespace Quiz.Master.Core.Models;

public record GameConfiguration
{
    public string? StartVideoFileId { get; set; }
    public string? EndVideoFileId { get; set; }
    public string? StartScreenImageId  { get; set; }
}

