namespace Quiz.Master.Core.Models;

public record Player : IEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string DeviceId { get; set; }
    public DateTime CreatedAt { get; set; }
}
