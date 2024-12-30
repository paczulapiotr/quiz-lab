namespace Quiz.Master.Core.Models;

public enum GameStatus
{
    GameCreated = 1,
    GameJoined,
    GameStarting,
    GameStarted,
    RulesExplaining,
    RulesExplained,
    MiniGameStarting,
    MiniGameStarted,
    MiniGameEnding,
    MiniGameEnded,
    GameEnding,
    GameEnded,
}