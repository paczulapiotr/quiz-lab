using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Game.Round;

public interface IMiniGameHandler
{
    Task HandleMiniGame(MiniGameInstance game, CancellationToken cancellationToken = default);
}