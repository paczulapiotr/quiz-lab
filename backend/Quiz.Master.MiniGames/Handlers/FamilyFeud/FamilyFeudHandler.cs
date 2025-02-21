using Microsoft.Extensions.Options;
using Quiz.Master.Game.MiniGames;
using State = Quiz.Master.MiniGames.Models.FamilyFeud.FamilyFeudState;
using Definition = Quiz.Master.MiniGames.Models.FamilyFeud.FamilyFeudDefinition;
using Configuration = Quiz.Master.MiniGames.Models.FamilyFeud.Configuration;
using Quiz.Master.MiniGames.Handlers.FamilyFeud.Logic;
using FuzzySharp;

namespace Quiz.Master.MiniGames.Handlers.FamilyFeud;

public class FamilyFeudHandler(IMiniGameEventService eventService, IMiniGameRepository repository, IOptions<Configuration> options) : IMiniGameHandler
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
            var roundState = new State.RoundState { RoundId = round.Id };
            _state.Rounds.Add(roundState);

            playerRoundCounter = await RunRoundBase(round, playerRoundCounter, cancellationToken);
        }
    }

    private async Task<int> RunRoundBase(Definition.Round round, int playerRoundCounter, CancellationToken cancellationToken)
    {
        var gameId = _miniGame.GameId.ToString();
        var roundState = _state.Rounds.Find(x => x.RoundId == round.Id);

        if (roundState == null) throw new InvalidOperationException("Round state not found");

        var playerIds = _miniGame.PlayerIds.Select(x => x).ToArray();
        _state.CurrentRoundId = round.Id;
        await _onStateUpdate(_state, cancellationToken);

        await eventService.SendOnQuestionShow(gameId, cancellationToken);
        await eventService.WaitForQuestionShown(gameId, cancellationToken);

        while (roundState!.Answers.Count < round.Answers.Count())
        {
            var currentPlayerId = playerIds[playerRoundCounter++ % playerIds.Length];
            _state.CurrentGuessingPlayerId = currentPlayerId;
            await _onStateUpdate(_state, cancellationToken);

            await eventService.SendOnAnswerStart(gameId, cancellationToken);

            var answer = await new Selector(eventService)
                .Select(
                    gameId,
                    options: new(options.Value.TimeForAnswerMs, [_state.CurrentGuessingPlayerId.Value.ToString()]),
                    cancellationToken: cancellationToken);

            List<string> usedAnswers = roundState.Answers.Select(x => x.MatchedAnswerId!).ToList() ?? [];
            var matched = AnswerMatch.MatchAnswer(round.Answers, answer?.Answer ?? "", usedAnswers);

            var ans = matched?.answer;
            roundState?.Answers.Add(new()
            {
                PlayerId = currentPlayerId,
                Answer = answer?.Answer,
                MatchedAnswerId = ans?.Id,
                MatchedAnswer = matched?.synonym,
                Points = ans?.Points ?? 0,
                Timestamp = answer?.Timestamp
            });

            await _onStateUpdate(_state, cancellationToken);
            await _onPlayerScoreUpdate(currentPlayerId, ans?.Points ?? 0, cancellationToken);
            await eventService.SendOnAnswerShow(gameId, cancellationToken);
            await eventService.WaitForAnswerShown(gameId, cancellationToken);
        }

        await eventService.SendOnRoundEnd(gameId, cancellationToken);
        await eventService.WaitForRoundEnded(gameId, cancellationToken);
        return playerRoundCounter;
    }
}