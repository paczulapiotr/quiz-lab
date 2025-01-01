
using Microsoft.EntityFrameworkCore;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.CQRS;
using Quiz.Common.Messages.Game;
using Quiz.Master.Extensions;
using Quiz.Master.Persistance;

namespace Quiz.Master.Features.Game.JoinGame;

public record JoinGameCommand(Guid GameId, string PlayerName) : ICommand;

public class JoinGameHandler(IQuizRepository quizRepository, IPublisher publisher, IHttpContextAccessor httpContextAccessor) : ICommandHandler<JoinGameCommand>
{
    public async ValueTask<NoResult?> HandleAsync(JoinGameCommand request, CancellationToken cancellationToken = default)
    {
        var deviceId = httpContextAccessor.GetDeviceId();
        var game = await quizRepository.Query<Core.Models.Game>(true)
            .Include(x => x.Players)
            .FirstOrDefaultAsync(x => x.Id == request.GameId, cancellationToken);

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
            DeviceId = deviceId
        });

        await quizRepository.SaveChangesAsync(cancellationToken);
        await publisher.PublishAsync(new GameStatusUpdate(request.GameId.ToString(), GameStatus.GameJoined, deviceId), cancellationToken);

        if (game.Players.Count == game.GameSize)
        {
            await publisher.PublishAsync(new GameStatusUpdate(game.Id.ToString(), GameStatus.GameStarting), cancellationToken);
            Task.Delay(10_000)
                .ContinueWith(async (CancellationToken) =>
                {
                    await publisher.PublishAsync(new GameStatusUpdate(game.Id.ToString(), GameStatus.GameStarted));
                })
                .ConfigureAwait(false);
        }

        return await ValueTask.FromResult(NoResult.Instance);
    }
}
