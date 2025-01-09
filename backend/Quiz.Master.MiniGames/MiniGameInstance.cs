namespace Quiz.Master.MiniGames;

public record MiniGameInstance(Guid Id, Guid DefinitionId, Guid GameId, IEnumerable<Guid> PlayerIds);