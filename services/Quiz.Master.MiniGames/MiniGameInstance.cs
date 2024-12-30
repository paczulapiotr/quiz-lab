namespace Quiz.Master.MiniGames;

public record MiniGameInstance(Guid Id, Guid GameId, IEnumerable<string> PlayerIds);