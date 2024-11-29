using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Game.Round;

public class BlankGameRoundHandler : IGameRoundHandler
{
    public Task HandleRound(GameRound gameRound, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

}