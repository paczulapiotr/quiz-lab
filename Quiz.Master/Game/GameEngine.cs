
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
IGameRoundHandlerSelector gameRoundHandlerSelector,
IGameStateRepository gameStateRepository) : IGameEngine
{
    public bool IsGameFinished { get; private set; } = false;
    public bool IsGameRunning { get; private set; } = false;
    public Guid? CurrentGameId { get; private set; }
    public List<Player> Players { get; private set; } = [];
    public List<GameRound> GameRounds { get; private set; } = [];
    // Start game
    // - Pass players and chosen rounds/questions seed
    // - Intro welcome moview with rule desription

    public async Task Run(Guid Id, CancellationToken cancellationToken = default)
    {
        var gameEntity = await gameStateRepository.GetGame(Id, cancellationToken);
        IsGameRunning = true;
        CurrentGameId = Id;
        Players = gameEntity.Players.ToList();
        GameRounds = gameEntity.Rounds.ToList();
        var gameIdString = CurrentGameId.ToString()!;

        // - send welcome start rabbitmq message
        await communicationService.SendRulesExplainMessage(gameIdString, cancellationToken);
        // - wait for welcome finished rabbitmq message
        await communicationService.ReceiveRulesExplainedMessage(gameIdString, cancellationToken);

        foreach (var round in GameRounds)
        {
            await PlayRound(round, cancellationToken);
        }

        await communicationService.SendGameEndingMessage(gameIdString, cancellationToken);
        await communicationService.ReceiveGameEndedMessage(gameIdString, cancellationToken);

        IsGameRunning = false;
        IsGameFinished = true;
        await gameStateRepository.SaveGameState(this, cancellationToken);
    }

    private async Task PlayRound(GameRound round, CancellationToken cancellationToken = default)
    {
        var gameId = CurrentGameId.ToString();

        if (gameId is null)
        {
            throw new InvalidOperationException("Game not started");
        }

        // Start round
        // - pick round questions and handler
        var handler = await gameRoundHandlerSelector.GetHandler(round, cancellationToken);

        // - send round start rabbitmq message
        await communicationService.SendRoundStartingMessage(gameId, cancellationToken);

        // - wait for round start finished rabbitmq message
        await communicationService.ReceiveRoundStartedMessage(gameId, cancellationToken);

        // - invoke round handler
        // - wait for round handler to finish
        await handler.HandleRound(round, cancellationToken);

        // - send mid round ranking show rabbitmq message
        await communicationService.SendRoundEndingMessage(gameId, cancellationToken);

        // - wait for mid round ranking show finished rabbitmq message
        await communicationService.ReceiveRoundEndedMessage(gameId, cancellationToken);

        // - Save game state to db
        await gameStateRepository.SaveGameState(this, cancellationToken);
    }
}