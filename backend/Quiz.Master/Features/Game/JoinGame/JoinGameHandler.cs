
using Quiz.Common.Broker.Publisher;
using Quiz.Common.CQRS;
using Quiz.Common.Messages.Game;
using Quiz.Master.Extensions;
using Quiz.Storage;

namespace Quiz.Master.Features.Game.JoinGame;

public record JoinGameResult(bool Ok, string? ErrorCode = null);
public record JoinGameRequest(Guid GameId, string PlayerName) : IRequest<JoinGameResult>;

public class JoinGameHandler(IDatabaseStorage storage, IPublisher publisher, IHttpContextAccessor httpContextAccessor) : IRequestHandler<JoinGameRequest, JoinGameResult>
{
    public async ValueTask<JoinGameResult?> HandleAsync(JoinGameRequest request, CancellationToken cancellationToken = default)
    {
        var deviceId = httpContextAccessor.GetDeviceId();
        var game = await storage.FindGameAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return new JoinGameResult(false, "GameNotFound");
        }

        if (game.IsStarted)
        {
            return new JoinGameResult(false, "GameStarted");
        }

        if (game.Players.Any(x => x.DeviceId == deviceId))
        {
            return new JoinGameResult(false, "AlreadyJoined");
        }

        if (game.Players.Any(x => x.Name == request.PlayerName))
        {
            return new JoinGameResult(false, "NameAlreadyTaken");
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

        return new JoinGameResult(true);
    }
}
