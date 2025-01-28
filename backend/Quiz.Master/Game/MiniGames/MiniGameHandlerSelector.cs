using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Handlers.AbcdWithCategories;
using Quiz.Master.MiniGames.Handlers.MusicGuess;
using Quiz.Master.MiniGames.Handlers.PhrasesAndLetters;

namespace Quiz.Master.Game.MiniGames;

public class MiniGameHandlerSelector(IServiceScopeFactory scopeFactory) : IMiniGameHandlerSelector
{
    public Master.MiniGames.IMiniGameHandler GetHandler(MiniGameType miniGame, CancellationToken cancellationToken = default)
    {
        using var scope = scopeFactory.CreateScope();
        switch (miniGame)
        {
            case MiniGameType.AbcdWithCategories:
                return scope.ServiceProvider.GetRequiredService<AbcdWithCategoriesHandler>();
            case MiniGameType.MusicGuess:
                return scope.ServiceProvider.GetRequiredService<MusicGuessHandler>();
            case MiniGameType.PhrasesAndLetters:
                return scope.ServiceProvider.GetRequiredService<LettersAndPhrasesHandler>();
            default:
                throw new ArgumentOutOfRangeException(nameof(miniGame), miniGame, null);
        }
    }
}