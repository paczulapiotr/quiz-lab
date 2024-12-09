using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Game.Round;

public class MiniGameHandlerSelector : IMiniGameHandlerSelector
{
    public Task<IMiniGameHandler> GetHandler(MiniGameType miniGame, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IMiniGameHandler>(new BlankMiniGameHandler());
    }
}