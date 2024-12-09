
using Quiz.Common.Messages.Game;
using Quiz.Master.Game.Communication;
using Quiz.Master.Game.Repository;
using Quiz.Master.Game.Round;
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
IGameStateRepository gameStateRepository) : IGameEngine
{
    public bool IsGameFinished { get; private set; } = false;
    public bool IsGameRunning { get; private set; } = false;
    public Guid? CurrentGameId { get; private set; }
    public List<Player> Players { get; private set; } = [];
    public List<MiniGame> MiniGames { get; private set; } = [];
    // Start game
    // - Pass players and chosen rounds/questions seed
    // - Intro welcome moview with rule desription

    public async Task Run(Guid Id, CancellationToken cancellationToken = default)
    {
        var gameEntity = await gameStateRepository.GetGame(Id, cancellationToken);
        IsGameRunning = true;
        CurrentGameId = Id;
        Players = gameEntity.Players.ToList();
        MiniGames = gameEntity.MiniGames.ToList();
        var gameIdString = CurrentGameId.ToString()!;

        gameEntity.StartedAt = DateTime.UtcNow;
        await SetStatus(gameEntity, GameStatus.RulesExplaining, cancellationToken);
        await communicationService.SendRulesExplainMessage(gameIdString, cancellationToken);

        await SetStatus(gameEntity, GameStatus.RulesExplained, cancellationToken);
        await communicationService.ReceiveRulesExplainedMessage(gameIdString, cancellationToken);

        foreach (var miniGame in MiniGames)
        {
            gameEntity.CurrentMiniGame = miniGame.Type;
            await gameStateRepository.SaveGameState(gameEntity, cancellationToken);
            await PlayMiniGame(gameEntity, miniGame, cancellationToken);
        }

        gameEntity.CurrentMiniGame = null;
        await SetStatus(gameEntity, GameStatus.GameEnding, cancellationToken);
        await communicationService.SendGameEndingMessage(gameIdString, cancellationToken);

        gameEntity.FinishedAt = DateTime.UtcNow;
        await SetStatus(gameEntity, GameStatus.GameEnded, cancellationToken);
        await communicationService.ReceiveGameEndedMessage(gameIdString, cancellationToken);

        IsGameRunning = false;
        IsGameFinished = true;
    }

    private async Task SetStatus(Persistance.Models.Game game, GameStatus status, CancellationToken cancellationToken = default)
    {
        game.Status = status;
        await gameStateRepository.SaveGameState(game, cancellationToken);
    }

    private async Task PlayMiniGame(Persistance.Models.Game game, MiniGame miniGame, CancellationToken cancellationToken = default)
    {
        var gameId = CurrentGameId.ToString();

        if (gameId is null)
        {
            throw new InvalidOperationException("Game not started");
        }

        // - pick round questions and handler
        var handler = await miniGameHandlerSelector.GetHandler(miniGame.Type, cancellationToken);

        // - send round start rabbitmq message
        await communicationService.SendRoundStartingMessage(gameId, cancellationToken);

        // - wait for round start finished rabbitmq message
        await communicationService.ReceiveRoundStartedMessage(gameId, cancellationToken);

        // - wait for mini game handler to finish
        await handler.HandleMiniGame(miniGame, cancellationToken);

        // - send mid round ranking show rabbitmq message
        await communicationService.SendRoundEndingMessage(gameId, cancellationToken);

        // - wait for mid round ranking show finished rabbitmq message
        await communicationService.ReceiveRoundEndedMessage(gameId, cancellationToken);

        // - Save game state to db
        await gameStateRepository.SaveGameState(game, cancellationToken);
    }
}