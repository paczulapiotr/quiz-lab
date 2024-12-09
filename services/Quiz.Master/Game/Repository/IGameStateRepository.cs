namespace Quiz.Master.Game.Repository;

public interface IGameStateRepository
{
    Task SaveGameState(Persistance.Models.Game game, CancellationToken cancellationToken = default);
    Task<Persistance.Models.Game> GetGame(Guid gameId, CancellationToken cancellationToken = default);
}