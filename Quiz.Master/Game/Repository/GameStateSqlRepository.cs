namespace Quiz.Master.Game.Repository;

public class GameStateSqlRepository : IGameStateRepository
{
    public async Task SaveGameState(GameEngine game, CancellationToken cancellationToken = default)
    {
        // Save game state to SQL database
        await Task.CompletedTask;
    }
}
