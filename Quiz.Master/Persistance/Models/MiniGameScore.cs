
namespace Quiz.Master.Persistance.Models;

public record MiniGameScore : IEntity
{
    public Guid Id { get; set; }
    public Guid MiniGameId { get; set; }
    public required MiniGame MiniGame { get; set; }
    public Guid PlayerId { get; set; }
    public required Player Player { get; set; }
    public int Score { get; set; }

}

