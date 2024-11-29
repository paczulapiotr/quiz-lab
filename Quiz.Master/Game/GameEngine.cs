
using Quiz.Master.Game.Communication;
using Quiz.Master.Game.Repository;
using Quiz.Master.Game.Round;
using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Game;

public interface IGameEngine
{
    bool IsGameRunning { get; }
    bool IsGameFinished { get; }
    Task Run(Guid Id, List<Player> players, List<GameRound> gameRounds, CancellationToken cancellationToken = default);
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

    public async Task Run(Guid Id, List<Player> players, List<GameRound> gameRounds, CancellationToken cancellationToken = default)
    {
        IsGameRunning = true;
        CurrentGameId = Id;
        Players = players;
        GameRounds = gameRounds;

        // - send welcome start rabbitmq message
        await communicationService.SendRulesExplainMessage(Id.ToString(), cancellationToken);
        // - wait for welcome finished rabbitmq message
        await communicationService.ReceiveRulesExplainedMessage(cancellationToken);

        foreach (var round in GameRounds)
        {
            await PlayRound(round, cancellationToken);
        }

        await communicationService.SendGameEndMessage(Id.ToString(), cancellationToken);

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
        await communicationService.SendRoundStartMessage(gameId, cancellationToken);

        // - wait for round start finished rabbitmq message
        await communicationService.ReceiveRoundStartedMessage(cancellationToken);

        // - invoke round handler
        // - wait for round handler to finish
        await handler.HandleRound(round, cancellationToken);

        // - send mid round ranking show rabbitmq message
        await communicationService.SendRoundEndMessage(gameId, cancellationToken);

        // - wait for mid round ranking show finished rabbitmq message
        await communicationService.ReceiveRoundEndedMessage(cancellationToken);

        // - Save game state to db
        await gameStateRepository.SaveGameState(this, cancellationToken);
    }
}