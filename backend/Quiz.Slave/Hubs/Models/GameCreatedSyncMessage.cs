namespace Quiz.Slave.Hubs.Models;

public record GameCreatedSyncMessage(string GameId, uint GameSize);