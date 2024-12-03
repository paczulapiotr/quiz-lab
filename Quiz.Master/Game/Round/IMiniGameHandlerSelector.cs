using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Game.Round;

public interface IMiniGameHandlerSelector
{
    Task<IMiniGameHandler> GetHandler(MiniGameType miniGame, CancellationToken cancellationToken = default);
}
