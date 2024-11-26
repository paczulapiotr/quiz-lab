namespace Quiz.Slave.Hubs.Models;

public record PlayerJoinedSyncMessage(string DeviceId, string PlayerName);