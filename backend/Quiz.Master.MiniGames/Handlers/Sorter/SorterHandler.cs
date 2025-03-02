using Microsoft.Extensions.Options;
using Quiz.Master.Game.MiniGames;
using State = Quiz.Master.MiniGames.Models.Sorter.SorterState;
using Definition = Quiz.Master.MiniGames.Models.Sorter.SorterDefinition;
using Configuration = Quiz.Master.MiniGames.Models.Sorter.Configuration;

namespace Quiz.Master.MiniGames.Handlers.Sorter;

public class SorterHandler(IMiniGameEventService eventService, IMiniGameRepository repository, IOptions<Configuration> options) : IMiniGameHandler
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


        foreach (var round in definition.Rounds)
        {
            var roundState = new State.RoundState { RoundId = round.Id };
            _state.Rounds.Add(roundState);
            await RunRoundBase(round, cancellationToken);
        }
    }

    private async Task RunRoundBase(Definition.Round round, CancellationToken cancellationToken)
    {
        _state.CurrentRoundId = round.Id;
        await _onStateUpdate(_state, cancellationToken);
        var gameId = _miniGame.GameId.ToString();
        var playerIds = _miniGame.PlayerIds.Select(x => x.ToString()).ToArray();

        // Start round
        await eventService.SendOnRoundStart(gameId, _miniGame.IdString, cancellationToken);
        await eventService.WaitForRoundStarted(gameId, _miniGame.IdString, cancellationToken);

        // Start sorting
        await ProcessSortSelections(gameId, round, cancellationToken);

        // End round
        await eventService.SendOnRoundEnd(gameId, _miniGame.IdString, cancellationToken);
        await eventService.WaitForRoundSummary(gameId, _miniGame.IdString, cancellationToken);
    }

    private async Task ProcessSortSelections(string gameId, Definition.Round round, CancellationToken cancellationToken)
    {
        var roundState = _state.Rounds.FirstOrDefault(x => x.RoundId == _state.CurrentRoundId);
        var playerIds = _miniGame.PlayerIds.Select(x => x.ToString()).ToArray();
        var toSort = round.LeftCategory.Items.Select(x => x.Id)
            .Concat(round.RightCategory.Items.Select(x => x.Id));

        var awaitingSelections = new Dictionary<string, List<string>>();
        foreach (var playerId in playerIds)
        {
            awaitingSelections[playerId] = toSort.ToList();
        }

        var timedToken = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,
            new CancellationTokenSource(30_000_000).Token)
            .Token;

        while (!timedToken.IsCancellationRequested)
        {
            var selection = await eventService.GetSortSelection(gameId, timedToken);
            if (selection is not null)
            {
                var awaiting = awaitingSelections[selection.PlayerId.ToString()];
                awaiting.Remove(selection.ItemId);

                var answer = roundState?.Answers.FirstOrDefault(x => x.PlayerId == selection.PlayerId);
                var answerItem = new State.RoundAnswerItem
                {
                    CategoryItemId = selection.ItemId,
                    CategoryId = selection.CategoryId,
                    Timestamp = selection.Timestamp
                };

                if (answer is null)
                {
                    roundState?.Answers.Add(new State.RoundAnswer
                    {
                        PlayerId = selection.PlayerId,
                        Items = [answerItem],
                    });
                }
                else
                {
                    answer.Items.Add(answerItem);
                }

                if (awaitingSelections.All(x => x.Value.Count == 0))
                {
                    break;
                }
            }
        }

        var sorted = new Dictionary<string, List<string>>
        {
            { round.LeftCategory.Id, round.LeftCategory.Items.Select(x => x.Id).ToList() },
            { round.RightCategory.Id, round.RightCategory.Items.Select(x => x.Id).ToList() }
        };

        foreach (var playerAnswer in roundState?.Answers ?? [])
        {
            foreach (var answer in playerAnswer.Items)
            {
                if (sorted[answer.CategoryId].Contains(answer.CategoryItemId))
                {
                    playerAnswer.CorrectAnswers += 1;
                }
            }

            playerAnswer.Points = playerAnswer.CorrectAnswers * options.Value.PointsForCorrectAnswer;
            await _onPlayerScoreUpdate(playerAnswer.PlayerId, playerAnswer.Points, cancellationToken);
        }

        await _onStateUpdate(_state, cancellationToken);
    }
}