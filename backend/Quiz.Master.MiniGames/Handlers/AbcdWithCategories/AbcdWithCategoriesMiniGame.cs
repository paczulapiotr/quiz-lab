using System.Text.Json;
using Definition = Quiz.Master.MiniGames.Models.AbcdCategories.AbcdWithCategoriesDefinition;
using State = Quiz.Master.MiniGames.Models.AbcdCategories.AbcdWithCategoriesState;
using Quiz.Master.MiniGames.Models.AbcdCategories;

namespace Quiz.Master.MiniGames.Handlers.AbcdWithCategories;

public class AbcdWithCategoriesHandler(IMiniGameEventService eventService) : IMiniGameHandler
{
    private State _state = new State();
    private MiniGameInstance _miniGameInstance { get; set; } = null!;
    private Definition _definition { get; set; } = null!;
    private string _gameId => _miniGameInstance?.GameId.ToString() ?? string.Empty;
    private IEnumerable<string> _playerIds => _miniGameInstance.PlayerIds;
    private int _playersCount => _playerIds.Count();
    private PlayerScoreUpdateDelegate _onPlayerScoreUpdate { get; set; } = null!;
    private MiniGameStateUpdateDelegate _onStateUpdate { get; set; } = null!;

    public async Task<Dictionary<string, int>> Handle(
        MiniGameInstance game,
        string definitionJson,
        PlayerScoreUpdateDelegate onPlayerScoreUpdate,
        MiniGameStateUpdateDelegate onStateUpdate,
        CancellationToken cancellationToken = default)
    {
        _miniGameInstance = game;
        _onPlayerScoreUpdate = onPlayerScoreUpdate;
        _onStateUpdate = onStateUpdate;
        _definition = GetMiniGameDefinition(definitionJson);

        if (_definition is null)
        {
            throw new InvalidOperationException("Invalid mini game configuration");
        }
        var firstRoundDefinition = _definition.Rounds.FirstOrDefault();
        if (firstRoundDefinition is null)
        {
            throw new InvalidOperationException("Invalid mini game configuration - no rounds found");
        }
        var gameId = _miniGameInstance.GameId.ToString();
        await RunRoundBase(firstRoundDefinition.Id, cancellationToken);

        await eventService.SendOnPowerPlayExplain(gameId, cancellationToken);
        await eventService.WaitForPowerPlayExplained(gameId, cancellationToken);

        // For each question after first one
        foreach (var roundDefinition in _definition.Rounds.Skip(1))
        {
            // Save PowerPlay selection for every player
            var roundState = new State.RoundState { RoundId = roundDefinition.Id };
            _state.Rounds.Add(roundState);

            await SelectPowerPlays(roundDefinition.Id, cancellationToken);

            await RunRoundBase(roundDefinition.Id, cancellationToken);
        }

        return CalculatePoints();
    }

    private State.RoundState? GetRoundState(string roundId)
    {
        var roundState = _state.Rounds.FirstOrDefault(x => x.RoundId == roundId);
        if (roundState is null)
        {
            roundState = new State.RoundState { RoundId = roundId };
            _state.Rounds.Add(roundState);
        }
        return roundState;
    }

    private Definition.Round? GetRoundDefinition(string roundId)
    {
        var roundDefinition = _definition.Rounds.FirstOrDefault(x => x.Id == roundId);
        return roundDefinition;
    }

    private Dictionary<string, int> CalculatePoints()
    {
        var result = new Dictionary<string, int>();

        foreach (var round in _state.Rounds)
        {
            foreach (var answer in round.Answers)
            {
                if (result.ContainsKey(answer.DeviceId))
                {
                    result[answer.DeviceId] += answer.Points;
                }
                else
                {
                    result.Add(answer.DeviceId, answer.Points);
                }
            }
        }

        return result;
    }

    private async Task SelectPowerPlays(string roundId, CancellationToken cancellationToken)
    {
        await eventService.SendOnPowerPlayStart(_gameId, cancellationToken);
        var timeToken = new CancellationTokenSource(_definition.Config.TimeForPowerPlaySelectionMs).Token;
        var roundState = GetRoundState(roundId);
        var roundDefinition = GetRoundDefinition(roundId);

        // <deviceId, (PowerPlay, sourceDeviceId)[]>
        var powerPlays = new PowerPlaysDictionary();
        var categoryIds = roundDefinition?.Categories.Select(x => x.Id).ToList();

        for (int i = 0; i < _playersCount; i++)
        {
            var playersThatSelected = powerPlays.Values.SelectMany(x => x.Select(x => x.SourceDeviceId)).Distinct().ToArray();
            var selection = await eventService.GetPowerPlaySelection(_gameId, cancellationToken);

            if (selection is null || timeToken.IsCancellationRequested)
            {
                break;
            }

            if (powerPlays.ContainsKey(selection.DeviceId))
            {
                powerPlays[selection.DeviceId].Add(new(selection.DeviceId, selection.PowerPlay));
            }
            else
            {
                powerPlays.Add(selection.DeviceId, [new(selection.DeviceId, selection.PowerPlay)]);
            }
        }

        if (roundState is not null)
        {
            roundState.PowerPlays = powerPlays;
        }

        await _onStateUpdate(_state, cancellationToken);
        await eventService.SendOnPowerPlayApplication(_gameId, cancellationToken);
        await eventService.WaitForPowerPlayApplied(_gameId, cancellationToken);
    }

    private async Task RunRoundBase(string roundId, CancellationToken cancellationToken)
    {
        _state.CurrentRoundId = roundId;
        await _onStateUpdate(_state, cancellationToken);

        await eventService.SendOnCategorySelection(_gameId, cancellationToken);

        // Choose most voted category or random iftied
        await SelectCategory(roundId, cancellationToken);

        await eventService.SendOnCategorySelected(_gameId, cancellationToken);

        await eventService.WaitForCategoryPresented(_gameId, cancellationToken);

        await eventService.SendOnQuestionPresentation(_gameId, cancellationToken);

        await eventService.WaitForQuestionPresented(_gameId, cancellationToken);

        await eventService.SendOnQuestionSelection(_gameId, cancellationToken);

        await AnswerQuestion(roundId, cancellationToken);

        await eventService.SendOnQuestionAnswersPresentation(_gameId, cancellationToken);

        await eventService.WaitForQuestionAnswersPresented(_gameId, cancellationToken);
    }

    private async Task AnswerQuestion(string roundId, CancellationToken cancellationToken)
    {
        var config = _definition.Config;
        var timeToken = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,
            new CancellationTokenSource(config.TimeForAnswerSelectionMs).Token)
            .Token;

        var questionDefinition = _definition.Rounds
            .FirstOrDefault(x => _state.CurrentRoundId == x.Id)
            ?.Categories.FirstOrDefault(x => x.Id == _state.CurrentCategoryId)
            ?.Questions.FirstOrDefault(x => x.Id == _state.CurrentQuestionId);

        // <deviceId, (answerId, timestamp)>
        var answers = new Dictionary<string, (string answerId, DateTime timestamp)>();

        for (int i = 0; i < _playerIds.Count(); i++)
        {
            var answer = await eventService.GetQuestionAnswerSelection(_gameId, timeToken);

            if (answer is null || timeToken.IsCancellationRequested)
            {
                break;
            }

            answers.TryAdd(answer.DeviceId, (answer.AnswerId, answer.Timestamp));
        }

        var correctAnswerIds = questionDefinition?.Answers.Where(x => x.IsCorrect).Select(x => x.Id).ToList();
        var playerAnswers = _playerIds.Select(playerId =>
        {
            DateTime? timestamp = null;
            string answerId = string.Empty;
            if (answers.TryGetValue(playerId, out var answer))
            {
                timestamp = answer.timestamp;
                answerId = answer.answerId;
            }

            return new State.RoundAnswer
            {
                DeviceId = playerId,
                Points = 0,
                AnswerId = answerId,
                IsCorrect = string.IsNullOrWhiteSpace(answerId) ? false : correctAnswerIds?.Contains(answerId) ?? false,
                Timestamp = timestamp,
            };
        })
        .OrderBy(x => x.Timestamp)
        .ToList();

        var correctAnswers = 0;
        foreach (var ans in playerAnswers)
        {
            if (ans.IsCorrect)
            {
                ans.Points = Math.Max(
                    config.MaxPointsForAnswer - correctAnswers * config.PointsDecrement,
                    config.MinPointsForAnswer);
                correctAnswers++;

                var playerId = _playerIds.FirstOrDefault(id => id == ans.DeviceId);

                if (playerId is not null)
                {
                    await _onPlayerScoreUpdate(playerId, ans.Points, cancellationToken);
                }
            }
        }

        var roundState = GetRoundState(roundId);
        if (roundState is not null)
        {
            roundState.Answers = playerAnswers;
            await _onStateUpdate(_state, cancellationToken);
        }
    }

    private async Task SelectCategory(string roundId, CancellationToken cancellationToken)
    {
        var gameId = _miniGameInstance.GameId.ToString();
        var roundState = GetRoundState(roundId);
        var roundDefinition = GetRoundDefinition(roundId);
        var config = _definition.Config;
        var selections = new Dictionary<string, List<string>>();
        var timeToken = new CancellationTokenSource(config.TimeForCategorySelectionMs).Token;
        // <categoryId, deviceId[]>
        var categoryIds = roundDefinition?.Categories.Select(x => x.Id).ToList();

        for (int i = 0; i < _playersCount; i++)
        {
            var playersThatSelected = selections.Values.SelectMany(x => x).Distinct().ToArray();
            var selection = await eventService.GetCategorySelection(gameId, timeToken);

            if (selection is null || timeToken.IsCancellationRequested)
            {
                break;
            }

            if (selections.ContainsKey(selection.CategoryId))
            {
                selections[selection.CategoryId].Add(selection.DeviceId);
            }
            else
            {
                selections.Add(selection.CategoryId, [selection.DeviceId]);
            }
        }

        // add to state
        if (roundState is not null)
        {
            roundState.SelectedCategories = selections.Select(x => new State.SelectedCategory
            {
                CategoryId = x.Key,
                DeviceIds = x.Value
            }).ToList();
        }

        var selectedCategoryId = selections.OrderByDescending(x => x.Value.Count).FirstOrDefault().Key
            ?? categoryIds?[new Random().Next(categoryIds.Count)];

        var selectedCategory = roundDefinition?.Categories.FirstOrDefault(x => x.Id == selectedCategoryId);
        var questionDefinition = selectedCategory?.Questions.FirstOrDefault();

        _state.CurrentCategoryId = selectedCategory?.Id;
        _state.CurrentQuestionId = questionDefinition?.Id;

        await _onStateUpdate(_state, cancellationToken);
    }

    private Definition GetMiniGameDefinition(string definitionJson) => JsonSerializer.Deserialize<Definition>(definitionJson) ?? throw new InvalidOperationException("Invalid mini game configuration");
}