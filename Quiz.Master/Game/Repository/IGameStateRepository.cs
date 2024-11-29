namespace Quiz.Master.Game.Repository;

public interface IGameStateRepository
{
    Task SaveGameState(GameEngine game, CancellationToken cancellationToken = default);
}
