namespace Quiz.Slave.Hubs.Models;

public record MiniGameNotificationSyncMessage(string GameId, string MiniGameType, string Action, Dictionary<string, string>? Metadata);