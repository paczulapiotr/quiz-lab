using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Game.MiniGames;

public class MiniGameHandlerSelector(AbcdWithCategoriesMiniGame abcdWithCategoriesMiniGame) : IMiniGameHandlerSelector
{
    public Task<IMiniGameHandler> GetHandler(MiniGameType miniGame, CancellationToken cancellationToken = default)
    {
        switch (miniGame)
        {
            case MiniGameType.AbcdWithCategories:
                return Task.FromResult<IMiniGameHandler>(abcdWithCategoriesMiniGame);
            default:
                throw new ArgumentOutOfRangeException(nameof(miniGame), miniGame, null);
        }
    }
}