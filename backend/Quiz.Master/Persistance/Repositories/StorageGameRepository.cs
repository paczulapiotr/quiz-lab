
using Quiz.Master.Core.Models;
using Quiz.Master.Persistance.Repositories.Abstract;
using Quiz.Storage;

namespace Quiz.Master.Persistance.Repositories;

public class StorageGameRepository(IDatabaseStorage storage) : IGameRepository
{
    public async Task<Core.Models.Game> FindGameAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        return await storage.FindGameAsync(gameId, cancellationToken);
    }

    public async Task<GameDefinition> FindGameDefinitionAsync(Guid gameDefinitionId, CancellationToken cancellationToken = default)
    {
        return await storage.FindGameDefinitionAsync(gameDefinitionId, cancellationToken);
    }

    public async Task UpdateAsync(Core.Models.Game game, CancellationToken cancellationToken = default)
    {
        await storage.UpdateGameAsync(game, cancellationToken);
    }
}
