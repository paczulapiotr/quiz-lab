using Microsoft.Extensions.Options;
using Quiz.Master.Game.MiniGames;
using Quiz.Master.MiniGames.Models.MusicGuess;

namespace Quiz.Master.MiniGames.Handlers.MusicGuess;

public class MusicGuessMiniGame(IMiniGameEventService eventService, IMiniGameRepository repository, IOptions<Configuration> options) : IMiniGameHandler
{
    public Task<Dictionary<Guid, int>> Handle(MiniGameInstance game, PlayerScoreUpdateDelegate onPlayerScoreUpdate, MiniGameStateUpdateDelegate onStateUpdate, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}