
namespace Quiz.Master.Persistance.Models;

public record MiniGameInstanceScore : IEntity
{
    public Guid Id { get; set; }
    public Guid MiniGameInstanceId { get; set; }
    public MiniGameInstance? MiniGameInstance { get; set; }
    public Guid PlayerId { get; set; }
    public Player? Player { get; set; }
    public int Score { get; set; }

}

