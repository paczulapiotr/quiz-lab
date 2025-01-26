
using Quiz.Master.Core.Models;
using Quiz.Master.Game.Communication;
using Quiz.Master.Game.MiniGames;
using Quiz.Master.Persistance.Repositories.Abstract;
using Status = Quiz.Master.Core.Models.GameStatus;

namespace Quiz.Master.Game;

public interface IGameEngine
{
    Task Run(Guid Id, CancellationToken cancellationToken = default);
}

public class GameEngine(
ICommunicationService communicationService,
IMiniGameHandlerSelector miniGameHandlerSelector,
IGameRepository gameRepository,
IMiniGameRepository miniGameRepository) : IGameEngine
{
    public async Task Run(Guid Id, CancellationToken cancellationToken = default)
    {
        await communicationService.Initialize(Id.ToString(), cancellationToken);
        var game = await StartGame(Id, cancellationToken);
        var gameDefinition = await gameRepository.FindGameDefinitionAsync(game.GameDefinitionId, cancellationToken);
        
        foreach (var definition in gameDefinition.MiniGames)
        {
            await PlayMiniGame(game, definition, cancellationToken);
        }

        await FinishGame(game, cancellationToken);
    }

    private async Task<Core.Models.Game> StartGame(Guid id, CancellationToken cancellationToken)
    {
        var gameId = id.ToString()!;
        await communicationService.SendGameCreatedMessage(gameId, cancellationToken);
        await communicationService.ReceiveGameStartedMessage(gameId, cancellationToken);

        var game = await gameRepository.FindAsync(id, cancellationToken);
        game.StartedAt = DateTime.UtcNow;
        await SetStatus(game, Status.RulesExplaining, cancellationToken);
        await communicationService.SendRulesExplainMessage(gameId, cancellationToken);
        await communicationService.ReceiveRulesExplainedMessage(gameId, cancellationToken);
        await SetStatus(game, Status.RulesExplained, cancellationToken);

        return game!;
    }

    private async Task FinishGame(Core.Models.Game gameEntity, CancellationToken cancellationToken)
    {
        var gameId = gameEntity.Id.ToString()!;
        gameEntity.CurrentMiniGameId = null;
        gameEntity.FinishedAt = DateTime.UtcNow;
        await SetStatus(gameEntity, Status.GameEnding, cancellationToken);
        await communicationService.SendGameEndingMessage(gameId, cancellationToken);
        await communicationService.ReceiveGameEndedMessage(gameId, cancellationToken);
        await SetStatus(gameEntity, Status.GameEnded, cancellationToken);
    }

    private async Task SetStatus(Core.Models.Game game, Status status, CancellationToken cancellationToken = default)
    {
        game.Status = status;
        await gameRepository.UpdateAsync(game, cancellationToken: cancellationToken);
    }

    private async Task PlayMiniGame(Core.Models.Game game, SimpleMiniGameDefinition definition, CancellationToken cancellationToken = default)
    {
        var miniGame = await CreateMiniGame(definition, game.Id, cancellationToken);
        game.CurrentMiniGameId = miniGame.Id;
        await gameRepository.UpdateAsync(game, cancellationToken: cancellationToken);

        var gameId = game.Id.ToString();

        if (gameId is null)
        {
            throw new InvalidOperationException("Game not started");
        }

        // - pick round questions and handler
        var handler = miniGameHandlerSelector.GetHandler(miniGame.Type, cancellationToken);

        // - send mini game start rabbitmq message
        await SetStatus(game, Status.MiniGameStarting, cancellationToken);
        await communicationService.SendMiniGameStartingMessage(gameId, cancellationToken);
        await communicationService.ReceiveMiniGameStartedMessage(gameId, cancellationToken);
        await SetStatus(game, Status.MiniGameStarted, cancellationToken);

        var playerIds = game.Players.Select(x => x.Id).ToList();

        // - wait for mini game handler to finish
        await handler.Handle(
            new Master.MiniGames.MiniGameInstance(miniGame.Id, miniGame.MiniGameDefinitionId, game.Id, playerIds),
            (playerId, score, token) => miniGameRepository.UpsertPlayerScoreAsync(miniGame.Id, playerId, score, token),
            (state, token) => miniGameRepository.UpdateMiniGameStateAsync(miniGame.Id, state, token),
            cancellationToken);

        await gameRepository.UpdateAsync(game, cancellationToken: cancellationToken);


        // - send mini game ening msg for score table rabbitmq message
        await SetStatus(game, Status.MiniGameEnding, cancellationToken);
        await communicationService.SendMiniGameEndingMessage(gameId, cancellationToken);
        await communicationService.ReceiveMiniGameEndedMessage(gameId, cancellationToken);
        await SetStatus(game, Status.MiniGameEnded, cancellationToken);
    }

    private async Task<MiniGameInstance> CreateMiniGame(SimpleMiniGameDefinition definition, Guid gameId, CancellationToken cancellationToken)
    {
        var miniGame = new MiniGameInstance
        {
            GameId = gameId,
            MiniGameDefinitionId = definition.MiniGameDefinitionId,
            Type = definition.Type,
            PlayerScores = new List<MiniGameInstanceScore>()
        };

        await miniGameRepository.InsertMiniGameAsync(miniGame, cancellationToken);
        return miniGame;
    }
}