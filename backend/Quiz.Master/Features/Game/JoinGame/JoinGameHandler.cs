
using Quiz.Common.Broker.Publisher;
using Quiz.Common.CQRS;
using Quiz.Common.Messages.Game;
using Quiz.Master.Extensions;
using Quiz.Storage;

namespace Quiz.Master.Features.Game.JoinGame;

public record JoinGameCommand(Guid GameId, string PlayerName) : ICommand;

public class JoinGameHandler(IDatabaseStorage storage, IPublisher publisher, IHttpContextAccessor httpContextAccessor) : ICommandHandler<JoinGameCommand>
{
    public async ValueTask<NoResult?> HandleAsync(JoinGameCommand request, CancellationToken cancellationToken = default)
    {
        var deviceId = httpContextAccessor.GetDeviceId();
        var game = await storage.FindGameAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            throw new InvalidOperationException("Game not found");
        }

        if (game.IsStarted)
        {
            throw new InvalidOperationException("Game already started");
        }

        if (game.Players.Any(x => x.DeviceId == deviceId))
        {
            throw new InvalidOperationException("Player already joined");
        }

        game.Players.Add(new Core.Models.Player
        {
            Name = request.PlayerName,
            DeviceId = deviceId,
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
        });

        await storage.UpdateGameAsync(game, cancellationToken);

        var gameId = request.GameId.ToString();
        await publisher.PublishAsync(new GameStatusUpdate(gameId, GameStatus.GameJoined, deviceId), gameId, cancellationToken);

        if (game.Players.Count == game.GameSize)
        {
            await publisher.PublishAsync(new GameStatusUpdate(gameId, GameStatus.GameStarting), cancellationToken: cancellationToken);
        }

        return await ValueTask.FromResult(NoResult.Instance);
    }
}
