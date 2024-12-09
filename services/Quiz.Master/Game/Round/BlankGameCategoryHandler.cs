using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Game.Round;

public class BlankMiniGameHandler : IMiniGameHandler
{
    public Task HandleMiniGame(MiniGameInstance game, CancellationToken cancellationToken = default)
    {
        return Task.Delay(10_000);
    }
}