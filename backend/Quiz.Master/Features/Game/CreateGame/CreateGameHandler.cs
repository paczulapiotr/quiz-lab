
using Quiz.Common.Broker.Publisher;
using Quiz.Common.CQRS;
using Quiz.Common.Messages.Game;
using Quiz.Storage;
using GameStatus = Quiz.Master.Core.Models.GameStatus;

namespace Quiz.Master.Features.Game.CreateGame;

public record CreateGameCommand(uint GameSize, Guid GameDefinitionId) : ICommand;

public class CreateGameHandler : ICommandHandler<CreateGameCommand>
{
    private readonly IPublisher publisher;
    private readonly IDatabaseStorage storage;

    public CreateGameHandler(IDatabaseStorage storage, IPublisher publisher)
    {
        this.storage = storage;
        this.publisher = publisher;
    }
    public async ValueTask<NoResult?> HandleAsync(CreateGameCommand request, CancellationToken cancellationToken = default)
    {
        var game = new Core.Models.Game
        {
            GameSize = request.GameSize,
            GameDefinitionId = request.GameDefinitionId,
            Status = GameStatus.GameCreated,
            CreatedAt = DateTime.UtcNow,
        };
        await storage.InsertGameAsync(game, cancellationToken);

        var gameId = game.Id.ToString();
        await publisher.PublishAsync(new GameStatusUpdate(gameId, Common.Messages.Game.GameStatus.GameCreated), cancellationToken: cancellationToken);

        return NoResult.Instance;
    }
}
