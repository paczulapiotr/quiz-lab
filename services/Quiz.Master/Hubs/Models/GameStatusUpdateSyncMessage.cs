using Quiz.Common.Messages.Game;

namespace Quiz.Master.Hubs.Models;

public record GameStatusUpdateSyncMessage(string GameId, GameStatus Status);