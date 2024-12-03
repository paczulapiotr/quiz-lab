using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Game.Round;

public interface IMiniGameHandler
{
    Task HandleMiniGame(MiniGame game, CancellationToken cancellationToken = default);
}