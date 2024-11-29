
namespace Quiz.Master.Game.Repository;

public class GameStateSqlRepository : IGameStateRepository
{
    public Task<Persistance.Models.Game> GetGame(Guid gameId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new Persistance.Models.Game());
    }

    public async Task SaveGameState(GameEngine game, CancellationToken cancellationToken = default)
    {
        // Save game state to SQL database
        await Task.CompletedTask;
    }
}
