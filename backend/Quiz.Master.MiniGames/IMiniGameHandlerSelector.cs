
using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames;

namespace Quiz.Master.Game.MiniGames;

public interface IMiniGameHandlerSelector
{
    IMiniGameHandler GetHandler(MiniGameType miniGame, CancellationToken cancellationToken = default);
}
