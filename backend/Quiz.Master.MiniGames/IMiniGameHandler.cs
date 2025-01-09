
using Quiz.Master.Core.Models;

namespace Quiz.Master.MiniGames;

public interface IMiniGameHandler
{
    // <playerId, score>
    Task<Dictionary<Guid, int>> Handle(
        MiniGameInstance game,
        PlayerScoreUpdateDelegate onPlayerScoreUpdate,
        MiniGameStateUpdateDelegate onStateUpdate,
        CancellationToken cancellationToken = default);
}

public delegate Task PlayerScoreUpdateDelegate(Guid playerId, int score, CancellationToken cancellationToken);
public delegate Task MiniGameStateUpdateDelegate(MiniGameStateData state, CancellationToken cancellationToken);
