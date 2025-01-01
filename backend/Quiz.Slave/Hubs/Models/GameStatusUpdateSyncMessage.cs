using Quiz.Common.Messages.Game;

namespace Quiz.Slave.Hubs.Models;

public record GameStatusUpdateSyncMessage(string GameId, GameStatus Status);