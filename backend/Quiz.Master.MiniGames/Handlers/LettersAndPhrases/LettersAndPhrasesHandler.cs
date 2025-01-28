using Microsoft.Extensions.Options;
using Quiz.Master.Game.MiniGames;
using State = Quiz.Master.MiniGames.Models.LettersAndPhrases.LettersAndPhrasesState;
using Definition = Quiz.Master.MiniGames.Models.LettersAndPhrases.LettersAndPhrasesDefinition;
using Configuration = Quiz.Master.MiniGames.Models.LettersAndPhrases.Configuration;
using Quiz.Master.MiniGames.Handlers.PhrasesAndLetters.Logic;

namespace Quiz.Master.MiniGames.Handlers.PhrasesAndLetters;

public class LettersAndPhrasesHandler(IMiniGameEventService eventService, IMiniGameRepository repository, IOptions<Configuration> options) : IMiniGameHandler
{
    private MiniGameInstance _miniGame = null!;
    private State _state = new State();
    private PlayerScoreUpdateDelegate _onPlayerScoreUpdate = null!;
    private MiniGameStateUpdateDelegate _onStateUpdate = null!;

    public async Task Handle(MiniGameInstance miniGame, PlayerScoreUpdateDelegate onPlayerScoreUpdate, MiniGameStateUpdateDelegate onStateUpdate, CancellationToken cancellationToken = default)
    {
        _miniGame = miniGame;
        _onPlayerScoreUpdate = onPlayerScoreUpdate;
        _onStateUpdate = onStateUpdate;
        var definition = (await repository.FindMiniGameDefinitionAsync(miniGame.DefinitionId, cancellationToken)).Definition.As<Definition>();
        if (definition == null)
        {
            throw new InvalidOperationException("Mini game definition not found");
        }
        await eventService.Initialize(miniGame.GameId.ToString(), cancellationToken);

        var playerRoundCounter = 0;

        foreach (var round in definition.Rounds)
        {
            if(playerRoundCounter != 0)
            {
                playerRoundCounter++;
            }

            var roundState = new State.RoundState { RoundId = round.Id };
            _state.Rounds.Add(roundState);
            playerRoundCounter = await RunRoundBase(round, playerRoundCounter, cancellationToken);
        }
    }

    private async Task<int> RunRoundBase(Definition.Round round, int playerRoundCounter, CancellationToken cancellationToken)
    {
        var gameId = _miniGame.GameId.ToString();
        _state.CurrentRoundId = round.Id;
        await _onStateUpdate(_state, cancellationToken);
        var roundState = _state.Rounds.Find(x => x.RoundId == _state.CurrentRoundId);
        var playerIds = _miniGame.PlayerIds.Select(x => x).ToArray();

        await eventService.SendOnQuestionShow(gameId, cancellationToken);
        await eventService.WaitForQuestionShown(gameId, cancellationToken);


        while (GetAvailableLetters(round, roundState).Count() > 0)
        {
            var playerId = playerIds[playerRoundCounter % playerIds.Length];
            _state.CurrentGuessingPlayerId = playerId;
            await _onStateUpdate(_state, cancellationToken);
            await eventService.SendOnAnswerStart(gameId, cancellationToken);

            var letter = await SelectLetter(gameId, playerId, cancellationToken);

            var availableLetters = GetAvailableLetters(round, roundState);

            var isCorrect = availableLetters?.Contains(letter?.Letter ?? '-') ?? false;
            var points = isCorrect ? options.Value.PointsForAnswer : 0;

            roundState?.Answers.Add(new State.RoundAnswer
            {
                Letter = letter?.Letter ?? '-',
                PlayerId = playerId,
                Timestamp = letter?.Timestamp,
                IsCorrect = isCorrect,
                Points = points
            });

            await _onStateUpdate(_state, cancellationToken);
            await _onPlayerScoreUpdate(playerId, points, cancellationToken);
            await eventService.SendOnAnswered(gameId, cancellationToken);

            if (!isCorrect)
            {
                playerRoundCounter++;
            }
        }

        await eventService.SendOnPhraseSolvedPresentation(gameId, cancellationToken);
        await eventService.WaitForPhraseSolvedPresented(gameId, cancellationToken);

        return playerRoundCounter;
    }

    private static IEnumerable<char> GetAvailableLetters(Definition.Round round, State.RoundState? roundState)
    {
        var phraseLetters = round.Phrase.Replace(" ", "").ToCharArray().Distinct() ?? [];
        var selectedLetters = roundState?.Answers.Select(x => x.Letter).Distinct() ?? [];

        var availableLetters = phraseLetters.Except(selectedLetters);
        return availableLetters ?? [];
    }

    private async Task<IMiniGameEventService.AnswerSelection?> SelectLetter(string gameId, Guid playerId, CancellationToken cancellationToken)
    {
        return await new LetterSelector(eventService).Select(gameId, null, new(options.Value.TimeForAnswerMs, [playerId.ToString()]), cancellationToken);
    }
}