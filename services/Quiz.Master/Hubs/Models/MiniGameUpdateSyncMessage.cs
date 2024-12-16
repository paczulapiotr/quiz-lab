namespace Quiz.Master.Hubs.Models;

public record MiniGameUpdateSyncMessage(string GameId, string MiniGameType, string Action, string? Value, Dictionary<string, string>? Data);