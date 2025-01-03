using Quiz.Common.Messages.Game;
using Quiz.Master.Hubs.Models;

namespace Quiz.Master.Hubs;

internal partial class SyncHub
{
    public async Task MiniGameUpdate(MiniGameUpdateSyncMessage message)
    {
        await _publisher.PublishAsync(new MiniGameUpdate(message.GameId, message.MiniGameType, message.Action, message.Value, message.Data), message.GameId);
    }
}