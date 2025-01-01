
namespace Quiz.Master.MiniGames;

public interface IMiniGameHandler
{
    // <deviceId, score>
    Task<Dictionary<string, int>> Handle(
        MiniGameInstance game,
        string definitionJson,
        PlayerScoreUpdateDelegate onPlayerScoreUpdate,
        MiniGameStateUpdateDelegate onStateUpdate,
        CancellationToken cancellationToken = default);
}

public delegate Task PlayerScoreUpdateDelegate(string playerId, int score, CancellationToken cancellationToken);
public delegate Task MiniGameStateUpdateDelegate(object state, CancellationToken cancellationToken);
