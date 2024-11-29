using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Game.Round;

public interface IGameRoundHandler
{
    Task HandleRound(GameRound gameRound, CancellationToken cancellationToken = default);
}