
using Quiz.Common.Broker.Publisher;
using Quiz.Common.CQRS;
using Quiz.Common.Messages.Game;
using Quiz.Master.Core.Models;
using Quiz.Storage;
using GameStatus = Quiz.Master.Core.Models.GameStatus;

namespace Quiz.Master.Features.Game.CreateGame;

public enum GameLanguage {
    Polish,
    English
}

public record CreateGameCommand(uint GameSize, string GameIdentifier, GameLanguage Language) : ICommand;

public class CreateGameHandler : ICommandHandler<CreateGameCommand>
{
    private readonly IPublisher publisher;
    private readonly IContentManagementClient contentClient;
    private readonly IDatabaseStorage storage;

    public CreateGameHandler(IDatabaseStorage storage, IPublisher publisher, IContentManagementClient contentClient)
    {
        this.storage = storage;
        this.publisher = publisher;
        this.contentClient = contentClient;
    }
    public async ValueTask<NoResult?> HandleAsync(CreateGameCommand request, CancellationToken cancellationToken = default)
    {
        var gameDefinitionId = await LoadGameDefinitionAsync(request, cancellationToken);
        var game = new Core.Models.Game
        {
            GameSize = request.GameSize,
            GameDefinitionId = gameDefinitionId,
            Status = GameStatus.GameCreated,
            CreatedAt = DateTime.UtcNow,
        };
        await storage.InsertGameAsync(game, cancellationToken);

        var gameId = game.Id.ToString();
        await publisher.PublishAsync(new NewGameCreation(gameId), cancellationToken: cancellationToken);

        return NoResult.Instance;
    }

    private async Task<Guid> LoadGameDefinitionAsync(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var gameDefinitionContent = await contentClient.GetGameDefinition(request.GameIdentifier, request.Language);
        var miniGames = gameDefinitionContent.MiniGames.ToList();
        await storage.InsertManyMiniGameDefinitionAsync(miniGames, cancellationToken);
        var gameDefinition = new GameDefinition
        {
            Identifier = gameDefinitionContent.Identifier,
            Name = gameDefinitionContent.Name,
            Locale = gameDefinitionContent.Locale,
            CreatedAt = DateTime.UtcNow,
            MiniGames = miniGames.Select(x => new SimpleMiniGameDefinition
            {
                MiniGameDefinitionId = x.Id,
                Type = x.Type
            })
        };

        await storage.InsertGameDefinitionAsync(gameDefinition, cancellationToken);

        return gameDefinition.Id;
    }
}
