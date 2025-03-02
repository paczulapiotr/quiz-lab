using Microsoft.Extensions.Options;
using Quiz.Master.Game.MiniGames;
using State = Quiz.Master.MiniGames.Models.LettersAndPhrases.LettersAndPhrasesState;
using Definition = Quiz.Master.MiniGames.Models.LettersAndPhrases.LettersAndPhrasesDefinition;
using Configuration = Quiz.Master.MiniGames.Models.LettersAndPhrases.Configuration;
using Quiz.Master.MiniGames.Handlers.LettersAndPhrases.Logic;

namespace Quiz.Master.MiniGames.Handlers.LettersAndPhrases;

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
            if (playerRoundCounter != 0)
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

        await eventService.SendOnQuestionShow(gameId, _miniGame.IdString, cancellationToken);
        await eventService.WaitForQuestionShown(gameId, _miniGame.IdString, cancellationToken);


        while (GetAvailableLetters(round, roundState).Count() > 0)
        {
            var playerId = playerIds[playerRoundCounter % playerIds.Length];
            _state.CurrentGuessingPlayerId = playerId;
            await _onStateUpdate(_state, cancellationToken);
            await eventService.SendOnAnswerStart(gameId, _miniGame.IdString, cancellationToken);

            var letter = await SelectLetter(gameId, playerId, cancellationToken);

            var availableLetters = GetAvailableLetters(round, roundState);

            var isCorrect = availableLetters?.Contains(letter?.Letter ?? '-') ?? false;
            var points = isCorrect ? options.Value.PointsForAnswer : 0;

            char? selectedLetter = letter?.Letter != null && char.IsLetter(letter.Letter)
                ? char.ToLower(letter.Letter)
                : null;

            roundState?.Answers.Add(new State.RoundAnswer
            {
                Letter = selectedLetter,
                PlayerId = playerId,
                Timestamp = letter?.Timestamp,
                IsCorrect = isCorrect,
                Points = points
            });

            await _onStateUpdate(_state, cancellationToken);
            await _onPlayerScoreUpdate(playerId, points, cancellationToken);
            await eventService.SendOnAnswered(gameId, _miniGame.IdString, cancellationToken);

            if (!isCorrect)
            {
                playerRoundCounter++;
            }
        }

        await eventService.SendOnPhraseSolvedPresentation(gameId, _miniGame.IdString, cancellationToken);
        await eventService.WaitForPhraseSolvedPresented(gameId, _miniGame.IdString, cancellationToken);

        return playerRoundCounter;
    }

    private static IEnumerable<char> GetAvailableLetters(Definition.Round round, State.RoundState? roundState)
    {
        var phraseLetters = round.Phrase.Replace(" ", "").ToLower().ToCharArray().Distinct() ?? [];
        var selectedLetters = roundState?.Answers
            .Where(x => x.Letter != null)
            .Select(x => x.Letter!.Value).Distinct() ?? [];

        var availableLetters = phraseLetters.Except(selectedLetters);
        return availableLetters ?? [];
    }

    private async Task<IMiniGameEventService.AnswerSelection?> SelectLetter(string gameId, Guid playerId, CancellationToken cancellationToken)
    {
        return await new LetterSelector(eventService).Select(gameId, null, new(options.Value.TimeForAnswerMs, [playerId.ToString()]), cancellationToken);
    }
}