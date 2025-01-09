namespace Quiz.Master.Persistance.Repositories.Abstract;

public interface IGameRepository
{
    Task UpdateAsync(Core.Models.Game game, CancellationToken cancellationToken = default);
    Task<Core.Models.Game> FindGameAsync(Guid gameId, CancellationToken cancellationToken = default);
    Task<Core.Models.GameDefinition> FindGameDefinitionAsync(Guid gameDefinitionId, CancellationToken cancellationToken = default);
}