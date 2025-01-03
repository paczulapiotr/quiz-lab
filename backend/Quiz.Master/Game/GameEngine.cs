
using Quiz.Master.Core.Models;
using Quiz.Master.Game.Communication;
using Quiz.Master.Game.MiniGames;
using Quiz.Master.Persistance.Repositories.Abstract;
using Status = Quiz.Master.Core.Models.GameStatus;

namespace Quiz.Master.Game;

public interface IGameEngine
{
    bool IsGameRunning { get; }
    bool IsGameFinished { get; }
    Task Run(Guid Id, CancellationToken cancellationToken = default);
}

public class GameEngine(
ICommunicationService communicationService,
IMiniGameHandlerSelector miniGameHandlerSelector,
IGameRepository gameRepository,
IMiniGameRepository miniGameRepository) : IGameEngine
{
    public bool IsGameFinished { get; private set; } = false;
    public bool IsGameRunning { get; private set; } = false;
    public Guid? CurrentGameId { get; private set; }
    public List<Player> Players { get; private set; } = [];
    public List<MiniGameInstance> MiniGames { get; private set; } = [];

    public async Task Run(Guid Id, CancellationToken cancellationToken = default)
    {
        await communicationService.Initialize(Id.ToString(), cancellationToken);
        var gameEntity = await StartGame(Id, cancellationToken);

        foreach (var miniGame in MiniGames)
        {
            await PlayMiniGame(gameEntity, miniGame, cancellationToken);
        }

        await FinishGame(gameEntity, cancellationToken);
    }

    private async Task<Core.Models.Game> StartGame(Guid id, CancellationToken cancellationToken)
    {
        var gameEntity = await gameRepository.FindAsync(id, cancellationToken);
        IsGameRunning = true;
        CurrentGameId = id;
        Players = gameEntity.Players.ToList();
        MiniGames = gameEntity.MiniGames.ToList();
        var gameIdString = CurrentGameId.ToString()!;

        gameEntity.StartedAt = DateTime.UtcNow;
        await SetStatus(gameEntity, Status.RulesExplaining, cancellationToken);
        await communicationService.SendRulesExplainMessage(gameIdString, cancellationToken);
        await communicationService.ReceiveRulesExplainedMessage(gameIdString, cancellationToken);
        await SetStatus(gameEntity, Status.RulesExplained, cancellationToken);

        return gameEntity!;
    }

    private async Task FinishGame(Core.Models.Game gameEntity, CancellationToken cancellationToken)
    {
        var gameId = CurrentGameId.ToString()!;

        gameEntity.CurrentMiniGame = null;
        gameEntity.FinishedAt = DateTime.UtcNow;
        await SetStatus(gameEntity, Status.GameEnding, cancellationToken);
        await communicationService.SendGameEndingMessage(gameId, cancellationToken);
        await communicationService.ReceiveGameEndedMessage(gameId, cancellationToken);
        await SetStatus(gameEntity, Status.GameEnded, cancellationToken);

        IsGameRunning = false;
        IsGameFinished = true;
    }

    private async Task SetStatus(Core.Models.Game game, Status status, CancellationToken cancellationToken = default)
    {
        game.Status = status;
        await gameRepository.UpdateAsync(game, cancellationToken: cancellationToken);
    }

    private async Task PlayMiniGame(Core.Models.Game game, MiniGameInstance miniGame, CancellationToken cancellationToken = default)
    {
        game.CurrentMiniGame = miniGame;
        await gameRepository.UpdateAsync(game, cancellationToken: cancellationToken);

        var gameId = CurrentGameId.ToString();

        if (gameId is null)
        {
            throw new InvalidOperationException("Game not started");
        }

        // - pick round questions and handler
        var handler = miniGameHandlerSelector.GetHandler(miniGame.MiniGameDefinition.Type, cancellationToken);

        // - send mini game start rabbitmq message
        await SetStatus(game, Status.MiniGameStarting, cancellationToken);
        await communicationService.SendMiniGameStartingMessage(gameId, cancellationToken);
        await communicationService.ReceiveMiniGameStartedMessage(gameId, cancellationToken);
        await SetStatus(game, Status.MiniGameStarted, cancellationToken);

        var playerIds = game.Players.Select(x => x.DeviceId).ToList();

        // - wait for mini game handler to finish
        await handler.Handle(
            new Master.MiniGames.MiniGameInstance(miniGame.Id, game.Id, playerIds),
            miniGame.MiniGameDefinition.DefinitionJsonData,
            (deviceId, score, token) => miniGameRepository.UpsertPlayerScoreAsync(miniGame.Id, deviceId, score, token),
            (state, token) => miniGameRepository.UpdateStateAsync(miniGame.Id, state, token),
            cancellationToken);

        await gameRepository.UpdateAsync(game, cancellationToken: cancellationToken);


        // - send mini game ening msg for score table rabbitmq message
        await SetStatus(game, Status.MiniGameEnding, cancellationToken);
        await communicationService.SendMiniGameEndingMessage(gameId, cancellationToken);
        await communicationService.ReceiveMiniGameEndedMessage(gameId, cancellationToken);
        await SetStatus(game, Status.MiniGameEnded, cancellationToken);
    }
}