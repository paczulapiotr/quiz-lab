namespace Quiz.Master.Core.Models;

public record Room : IEntity
{
    public Guid Id { get; set; }
    public string? GameId { get; set; }
    public required string Code { get; set; }
    public required string HostDeviceId { get; set; }
    public HashSet<string> PlayerDeviceIds { get; set; } = new HashSet<string>();
    public DateTime CreatedAt { get; set; }
    public bool IsOpen { get; set; }
}

