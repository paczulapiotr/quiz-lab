using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Game.Round;

public class GameRoundHandlerSelector : IGameRoundHandlerSelector
{
    public Task<IGameRoundHandler> GetHandler(GameRound gameRound, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IGameRoundHandler>(new BlankGameRoundHandler());
    }
}