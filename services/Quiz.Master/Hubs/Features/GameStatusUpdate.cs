using Quiz.Common.Messages.Game;
using Quiz.Master.Hubs.Models;

namespace Quiz.Master.Hubs;

internal partial class SyncHub
{
    public async Task GameStatusUpdate(GameStatusUpdateSyncMessage message)
    {
        await _publisher.PublishAsync(new GameStatusUpdate(message.GameId, message.Status));
    }
}