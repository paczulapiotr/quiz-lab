using Quiz.Master.Persistance;

namespace Quiz.Master.Game.Repository;

public interface IGameStateRepository
{
    Task Save<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, IEntity;
    Task<Persistance.Models.Game> GetGame(Guid gameId, CancellationToken cancellationToken = default);
}