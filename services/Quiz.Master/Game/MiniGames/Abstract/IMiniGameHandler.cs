using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Game.MiniGames;

public interface IMiniGameHandler
{
    Task HandleMiniGame(MiniGameInstance game, CancellationToken cancellationToken = default);
}