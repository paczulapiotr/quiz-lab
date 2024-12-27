
using Quiz.Common.Messages.Game;
using Quiz.Master.Game.Communication;
using Quiz.Master.Game.MiniGames;
using Quiz.Master.Game.Repository;
using Quiz.Master.Persistance.Models;

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
IGameStateRepository gameRepository) : IGameEngine
{
    public bool IsGameFinished { get; private set; } = false;
    public bool IsGameRunning { get; private set; } = false;
    public Guid? CurrentGameId { get; private set; }
    public List<Player> Players { get; private set; } = [];
    public List<MiniGameInstance> MiniGames { get; private set; } = [];
    // Start game
    // - Pass players and chosen rounds/questions seed
    // - Intro welcome moview with rule desription

    public async Task Run(Guid Id, CancellationToken cancellationToken = default)
    {
        var gameEntity = await StartGame(Id, cancellationToken);

        foreach (var miniGame in MiniGames)
        {
            await PlayMiniGame(gameEntity, miniGame, cancellationToken);
        }

        await FinishGame(gameEntity, cancellationToken);
    }

    private async Task<Persistance.Models.Game> StartGame(Guid id, CancellationToken cancellationToken)
    {
        var gameEntity = await gameRepository.GetGame(id, cancellationToken);
        IsGameRunning = true;
        CurrentGameId = id;
        Players = gameEntity.Players.ToList();
        MiniGames = gameEntity.MiniGames.ToList();
        var gameIdString = CurrentGameId.ToString()!;

        gameEntity.StartedAt = DateTime.UtcNow;
        await SetStatus(gameEntity, GameStatus.RulesExplaining, cancellationToken);
        await communicationService.SendRulesExplainMessage(gameIdString, cancellationToken);
        await communicationService.ReceiveRulesExplainedMessage(gameIdString, cancellationToken);
        await SetStatus(gameEntity, GameStatus.RulesExplained, cancellationToken);

        return gameEntity;
    }

    private async Task FinishGame(Persistance.Models.Game gameEntity, CancellationToken cancellationToken)
    {
        var gameId = CurrentGameId.ToString()!;

        gameEntity.CurrentMiniGame = null;
        gameEntity.FinishedAt = DateTime.UtcNow;
        await SetStatus(gameEntity, GameStatus.GameEnding, cancellationToken);
        await communicationService.SendGameEndingMessage(gameId, cancellationToken);
        await communicationService.ReceiveGameEndedMessage(gameId, cancellationToken);
        await SetStatus(gameEntity, GameStatus.GameEnded, cancellationToken);

        IsGameRunning = false;
        IsGameFinished = true;
    }

    private async Task SetStatus(Persistance.Models.Game game, GameStatus status, CancellationToken cancellationToken = default)
    {
        game.Status = status;
        await gameRepository.Save(game, cancellationToken: cancellationToken);
    }

    private async Task PlayMiniGame(Persistance.Models.Game game, MiniGameInstance miniGame, CancellationToken cancellationToken = default)
    {
        game.CurrentMiniGame = miniGame;
        await gameRepository.Save(game, cancellationToken: cancellationToken);

        var gameId = CurrentGameId.ToString();

        if (gameId is null)
        {
            throw new InvalidOperationException("Game not started");
        }

        // - pick round questions and handler
        var handler = await miniGameHandlerSelector.GetHandler(miniGame.MiniGameDefinition.Type, cancellationToken);

        // - send mini game start rabbitmq message
        await SetStatus(game, GameStatus.MiniGameStarting, cancellationToken);
        await communicationService.SendMiniGameStartingMessage(gameId, cancellationToken);
        await communicationService.ReceiveMiniGameStartedMessage(gameId, cancellationToken);
        await SetStatus(game, GameStatus.MiniGameStarted, cancellationToken);

        // - wait for mini game handler to finish
        await handler.HandleMiniGame(
            miniGame,
            cancellationToken);

        await gameRepository.Save(game, cancellationToken: cancellationToken);


        // - send mini game ening msg for score table rabbitmq message
        await SetStatus(game, GameStatus.MiniGameEnding, cancellationToken);
        await communicationService.SendMiniGameEndingMessage(gameId, cancellationToken);
        await communicationService.ReceiveMiniGameEndedMessage(gameId, cancellationToken);
        await SetStatus(game, GameStatus.MiniGameEnded, cancellationToken);
    }
}