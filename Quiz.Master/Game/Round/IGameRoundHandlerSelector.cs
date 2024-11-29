using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Game.Round;

public interface IGameRoundHandlerSelector
{
    Task<IGameRoundHandler> GetHandler(GameRound gameRound, CancellationToken cancellationToken = default);
}
