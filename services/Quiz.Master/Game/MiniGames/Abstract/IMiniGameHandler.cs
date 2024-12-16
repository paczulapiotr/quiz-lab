using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Game.MiniGames;

public interface IMiniGameHandler
{
    // <deviceId, score>
    Task<Dictionary<string, int>> HandleMiniGame(MiniGameInstance game, CancellationToken cancellationToken = default);
}

