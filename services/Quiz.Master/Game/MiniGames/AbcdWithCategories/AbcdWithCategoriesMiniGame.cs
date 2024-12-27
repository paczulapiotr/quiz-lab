using System.Text.Json;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;
using Quiz.Master.Persistance.Models;
using Quiz.Master.Persistance.Models.MiniGames.AbcdCategories;
using Quiz.Master.Persistance.Repositories.Abstract;
using Definition = Quiz.Master.Persistance.Models.MiniGames.AbcdCategories.AbcdWithCategoriesDefinition;
using State = Quiz.Master.Persistance.Models.MiniGames.AbcdCategories.AbcdWithCategoriesState;
using RoundDefinition = Quiz.Master.Persistance.Models.MiniGames.AbcdCategories.AbcdWithCategoriesDefinition.Round;
using QuestionDefintiion = Quiz.Master.Persistance.Models.MiniGames.AbcdCategories.AbcdWithCategoriesDefinition.Question;
using Quiz.Master.Game.Repository;

namespace Quiz.Master.Game.MiniGames;

public class AbcdWithCategoriesMiniGame(
    ILogger<AbcdWithCategoriesMiniGame> logger,
    IOneTimeConsumer<PlayerInteraction> playerInteraction,
    IOneTimeConsumer<MiniGameUpdate> miniGameUpdate,
    IPublisher publisher,
    IGameStateRepository gameStateRepository,
    IMiniGameSaveRepository miniGameSaveRepository) : IMiniGameHandler
{
    private State _state = new State();
    private MiniGameInstance _miniGameInstance { get; set; } = null!;
    private string _gameId => _miniGameInstance?.GameId.ToString() ?? string.Empty;
    private static string MiniGameType => Persistance.Models.MiniGameType.AbcdWithCategories.ToString();

    public async Task<Dictionary<string, int>> HandleMiniGame(MiniGameInstance game, CancellationToken cancellationToken = default)
    {
        _miniGameInstance = game;
        var miniGameDefinition = GetMiniGameDefinition();

        if (miniGameDefinition is null)
        {
            throw new InvalidOperationException("Invalid mini game configuration");
        }
        var config = miniGameDefinition.Config;
        var firstRoundDefinition = miniGameDefinition.Rounds.FirstOrDefault();
        if (firstRoundDefinition is null)
        {
            throw new InvalidOperationException("Invalid mini game configuration - no rounds found");
        }
        var gameId = _miniGameInstance.GameId.ToString();
        var firstRoundState = new State.RoundState { RoundId = firstRoundDefinition.Id };
        _state.Rounds.Add(firstRoundState);
        _state.CurrentRoundId = firstRoundDefinition.Id;

        await RunRoundBase(miniGameDefinition, firstRoundDefinition, firstRoundState, cancellationToken);

        // RABBIT_SEND start explaining PowerPlay
        await publisher.PublishAsync(new MiniGameNotification(gameId, MiniGameType, Action: "PowerPlayExplainStart"), cancellationToken);
        // RABBIT_RECEIVE stop explaining PowerPlay
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == "PowerPlayExplainStop", cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, MiniGameType, Action: "PowerPlayExplainStop"), cancellationToken);

        // For each question after first one
        foreach (var roundDefinition in miniGameDefinition.Rounds.Skip(1))
        {
            // Save PowerPlay selection for every player
            var roundState = new State.RoundState { RoundId = roundDefinition.Id };
            _state.Rounds.Add(roundState);
            await SelectPowerPlays(miniGameDefinition, roundDefinition, roundState, cancellationToken);

            // USE Round_Base
            await RunRoundBase(miniGameDefinition, roundDefinition, roundState, cancellationToken);
        }

        return CalculatePoints();
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

    private async Task SelectPowerPlays(Definition miniGameData, RoundDefinition round, State.RoundState roundState, CancellationToken cancellationToken)
    {
        // RABBIT_SEND start selecting PowerPlay
        await publisher.PublishAsync(new MiniGameNotification(_gameId, MiniGameType, Action: "PowerPlayStart"), cancellationToken);
        // HTTP_RECEIVE Receive ZAGRYWKA selection or wait for time to pass

        var timeToken = new CancellationTokenSource(miniGameData.Config.TimeForPowerPlaySelectionMs).Token;
        // <deviceId, (PowerPlay, sourceDeviceId)[]>
        var powerPlays = new PowerPlaysDictionary();
        var categoryIds = round.Categories.Select(x => x.Id).ToList();
        var playersCount = _miniGameInstance.Game.PlayersCount;
        for (int i = 0; i < playersCount; i++)
        {
            var playersThatSelected = powerPlays.Values.SelectMany(x => x.Select(x => x.SourceDeviceId)).Distinct().ToArray();
            var selection = await playerInteraction.ConsumeFirstAsync(
                condition: x => x.GameId == _gameId
                    && !playersThatSelected.Contains(x.DeviceId)
                    && x.InteractionType == "PowerPlaySelection",
                cancellationToken: timeToken);

            if (timeToken.IsCancellationRequested)
            {
                break;
            }
            try
            {
                var actionData = selection!.Data!;
                var deviceId = actionData["deviceId"]!;
                var powerPlay = Enum.Parse<PowerPlay>(actionData["powerPlay"]);

                if (powerPlays.ContainsKey(deviceId))
                {
                    powerPlays[deviceId].Add(new(selection.DeviceId, powerPlay));
                }
                else
                {
                    powerPlays.Add(deviceId, [new(selection.DeviceId, powerPlay)]);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error while parsing PowerPlay selection");
            }
        }

        roundState.PowerPlays = powerPlays;
        await SaveState(cancellationToken);

        // RABBIT_SEND start showing PowerPlays
        await publisher.PublishAsync(new MiniGameNotification(_gameId, MiniGameType, Action: "PowerPlayApplyStart"), cancellationToken);
        // RABBIT_RECEIVE stop showing PowerPlays
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == "PowerPlayApplyStop", cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(_gameId, MiniGameType, Action: "PowerPlayApplyStop"), cancellationToken);
    }

    private async Task RunRoundBase(Definition definition, RoundDefinition roundDefinition, State.RoundState roundState, CancellationToken cancellationToken)
    {
        var playersCount = _miniGameInstance.Game.PlayersCount;
        var config = definition!.Config;
        _state.CurrentRoundId = roundState.RoundId;
        await SaveState(cancellationToken);

        // RABBIT_SEND start selecting category
        await publisher.PublishAsync(new MiniGameNotification(_gameId, MiniGameType, Action: "CategorySelectStart"), cancellationToken);
        // Choose most voted category or random if tied
        var selectedCategoryId = await SelectCategoryId(_gameId, roundDefinition, playersCount, config.TimeForCategorySelectionMs, roundState);
        var selectedCategory = roundDefinition.Categories.FirstOrDefault(x => x.Id == selectedCategoryId);
        ArgumentException.ThrowIfNullOrWhiteSpace(selectedCategoryId, nameof(selectedCategoryId));
        var questionDefinition = selectedCategory!.Questions.First();
        roundState.CategoryId = selectedCategoryId;
        _state.CurrentCategoryId = selectedCategoryId;
        _state.CurrentQuestionId = questionDefinition.Id;
        await SaveState(cancellationToken);

        // RABBIT_SEND start showing selected category
        await publisher.PublishAsync(new MiniGameNotification(_gameId, MiniGameType, Action: "CategoryShowStart"), cancellationToken);

        // RABBIT_RECEIVE stop showing selected category
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == "CategoryShowStop", cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(_gameId, MiniGameType, "CategoryShowStop"), cancellationToken);

        // RABBIT_SEND start showing question
        await publisher.PublishAsync(new MiniGameNotification(_gameId, MiniGameType, Action: "QuestionShowStart"), cancellationToken);

        // RABBIT_RECEIVE stop showing question
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == "QuestionShowStop", cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(_gameId, MiniGameType, "QuestionShowStop"), cancellationToken);

        // RABBIT_SEND start countdown for answers
        await publisher.PublishAsync(new MiniGameNotification(_gameId, MiniGameType, Action: "QuestionAnswerStart"), cancellationToken);

        // RABBIT_RECEIVE stop countdown for answers
        // HTTP_RECEIVE Wait for answers or wait for time to pass
        await AnswerQuestion(definition.Config, questionDefinition, roundState);
        await SaveState(cancellationToken);

        // RABBIT_SEND start showing answers
        await publisher.PublishAsync(new MiniGameNotification(_gameId, MiniGameType, Action: "QuestionAnswerShowStart"), cancellationToken);

        // RABBIT_RECEIVE stop showing answers
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == "QuestionAnswerShowStop", cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(_gameId, MiniGameType, "QuestionAnswerShowStop"), cancellationToken);
    }

    private async Task AnswerQuestion(Definition.Configuration config, QuestionDefintiion question, State.RoundState roundState)
    {
        var playersCount = _miniGameInstance.Game.PlayersCount;
        var timeToken = new CancellationTokenSource(config.TimeForAnswerSelectionMs).Token;
        var playerIds = _miniGameInstance.Game.Players.Select(x => x.DeviceId.ToString()).ToList();
        // <deviceId, (answerId, timestamp)>
        var answers = new Dictionary<string, (string answerId, DateTime timestamp)>();

        for (int i = 0; i < playersCount; i++)
        {
            var answer = await playerInteraction.ConsumeFirstAsync(
                condition: x => x.GameId == _gameId
                    && playerIds.Contains(x.DeviceId)
                    && !answers.Keys.Contains(x.DeviceId)
                    && x.InteractionType == "QuestionAnswer"
                    && x.Value != null,
            cancellationToken: timeToken);

            if (timeToken.IsCancellationRequested)
            {
                break;
            }

            answers.TryAdd(answer!.DeviceId, (answer.Value!, answer.Timestamp));
        }

        var correctAnswerIds = question.Answers.Where(x => x.IsCorrect).Select(x => x.Id).ToList();
        var playerAnswers = _miniGameInstance.Game.Players.Select(p =>
        {
            DateTime? timestamp = null;
            string answerId = string.Empty;
            if (answers.TryGetValue(p.DeviceId, out var answer))
            {
                timestamp = answer.timestamp;
                answerId = answer.answerId;
            }

            return new State.RoundAnswer
            {
                DeviceId = p.DeviceId,
                Points = 0,
                AnswerId = answerId,
                IsCorrect = string.IsNullOrWhiteSpace(answerId) ? false : correctAnswerIds.Contains(answerId),
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

                var player = _miniGameInstance.Game.Players.FirstOrDefault(p => p.DeviceId == ans.DeviceId);

                if (player is not null)
                {
                    await miniGameSaveRepository.AddPlayerScore(_miniGameInstance.Id, player.Id, ans.Points);
                }
            }
        }

        roundState.Answers = playerAnswers;

        await SaveState();
    }

    private async Task<string> SelectCategoryId(string gameId, RoundDefinition firstRound, int playersCount, int timeForCategorySelectionMs, State.RoundState roundState)
    {
        var timeToken = new CancellationTokenSource(timeForCategorySelectionMs).Token;

        // <categoryId, deviceId[]>
        var selections = new Dictionary<string, List<string>>();
        var categoryIds = firstRound.Categories.Select(x => x.Id).ToList();
        for (int i = 0; i < playersCount; i++)
        {
            var playersThatSelected = selections.Values.SelectMany(x => x).Distinct().ToArray();
            var selection = await playerInteraction.ConsumeFirstAsync(
                condition: x => x.GameId == gameId
                    && !playersThatSelected.Contains(x.DeviceId)
                    && x.InteractionType == "CategorySelection"
                    && x.Value != null
                    && categoryIds.Contains(x.Value),
                cancellationToken: timeToken);

            if (timeToken.IsCancellationRequested)
            {
                break;
            }

            if (selections.ContainsKey(selection!.Value!))
            {
                selections[selection!.Value!].Add(selection!.DeviceId);
            }
            else
            {
                selections.Add(selection!.Value!, [selection!.DeviceId]);
            }
        }

        // add to state
        roundState.SelectedCategories = selections.Select(x => new State.SelectedCategory
        {
            CategoryId = x.Key,
            DeviceIds = x.Value
        }).ToList();

        await SaveState();

        var selectedCategoryId = selections.OrderByDescending(x => x.Value.Count).FirstOrDefault().Key
            ?? categoryIds[new Random().Next(categoryIds.Count)];

        return selectedCategoryId;
    }


    private async Task SaveState(CancellationToken cancellationToken = default)
    {
        await miniGameSaveRepository.SaveMiniGameState(_miniGameInstance.Id, _state, cancellationToken);
    }

    private Definition? GetMiniGameDefinition() => JsonSerializer.Deserialize<Definition>(_miniGameInstance.MiniGameDefinition.DefinitionJsonData);
}