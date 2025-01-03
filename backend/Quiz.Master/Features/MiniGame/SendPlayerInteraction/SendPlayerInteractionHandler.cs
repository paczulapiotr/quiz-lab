
using Quiz.Common.Broker.Publisher;
using Quiz.Common.CQRS;
using Quiz.Common.Messages.Game;

namespace Quiz.Master.Features.MiniGame.SendPlayerInteraction;

public record SendPlayerInteractionCommand(Guid GameId, string DeviceId, string InteractionType, string? Value = null, Dictionary<string, string>? Data = null) : ICommand;

public class SendPlayerInteractionHandler(IPublisher publisher) : ICommandHandler<SendPlayerInteractionCommand>
{
    public async ValueTask<NoResult?> HandleAsync(SendPlayerInteractionCommand request, CancellationToken cancellationToken = default)
    {
        var gameId = request.GameId.ToString();
        await publisher.PublishAsync(new PlayerInteraction(
            gameId,
            request.DeviceId,
            request.InteractionType,
            request.Value,
            request.Data), gameId, cancellationToken);

        return NoResult.Instance;
    }
}
