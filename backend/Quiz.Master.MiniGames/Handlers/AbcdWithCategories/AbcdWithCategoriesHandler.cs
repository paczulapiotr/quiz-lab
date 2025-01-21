using Definition = Quiz.Master.MiniGames.Models.AbcdCategories.AbcdWithCategoriesDefinition;
using State = Quiz.Master.MiniGames.Models.AbcdCategories.AbcdWithCategoriesState;
using Quiz.Master.MiniGames.Models.AbcdCategories;
using Quiz.Master.Game.MiniGames;
using Microsoft.Extensions.Options;
using Quiz.Master.MiniGames.Handlers.AbcdWithCategories.Logic;

namespace Quiz.Master.MiniGames.Handlers.AbcdWithCategories;

public class AbcdWithCategoriesHandler(IMiniGameEventService eventService, IMiniGameRepository repository, IOptions<Configuration> options) : IMiniGameHandler
{
    private State _state = new State();
    private MiniGameInstance _miniGameInstance { get; set; } = null!;
    private Definition _definition { get; set; } = null!;
    private string _gameId => _miniGameInstance?.GameId.ToString() ?? string.Empty;
    private IEnumerable<Guid> _playerIds => _miniGameInstance.PlayerIds;
    private PlayerScoreUpdateDelegate _onPlayerScoreUpdate { get; set; } = null!;
    private MiniGameStateUpdateDelegate _onStateUpdate { get; set; } = null!;

    public async Task Handle(
        MiniGameInstance game,
        PlayerScoreUpdateDelegate onPlayerScoreUpdate,
        MiniGameStateUpdateDelegate onStateUpdate,
        CancellationToken cancellationToken = default)
    {
        _miniGameInstance = game;
        _onPlayerScoreUpdate = onPlayerScoreUpdate;
        _onStateUpdate = onStateUpdate;
        var definition = (await repository.FindMiniGameDefinitionAsync(game.DefinitionId, cancellationToken)).Definition.As<Definition>();
        if (definition is null)
        {
            throw new InvalidOperationException("Invalid mini game configuration");
        }
        _definition = definition;
        var firstRoundDefinition = _definition.Rounds.FirstOrDefault();
        if (firstRoundDefinition is null)
        {
            throw new InvalidOperationException("Invalid mini game configuration - no rounds found");
        }
        await eventService.Initialize(_gameId, cancellationToken);
        await RunRoundBase(firstRoundDefinition.Id, cancellationToken);

        await eventService.SendOnPowerPlayExplain(_gameId, cancellationToken);
        await eventService.WaitForPowerPlayExplained(_gameId, cancellationToken);

        // For each question after first one
        foreach (var roundDefinition in _definition.Rounds.Skip(1))
        {
            // Save PowerPlay selection for every player
            var roundState = new State.RoundState { RoundId = roundDefinition.Id };
            _state.Rounds.Add(roundState);

            await eventService.SendOnPowerPlayStart(_gameId, cancellationToken);
            await SelectPowerPlays(roundDefinition.Id, cancellationToken);
            await eventService.SendOnPowerPlayApplication(_gameId, cancellationToken);
            await eventService.WaitForPowerPlayApplied(_gameId, cancellationToken);

            await RunRoundBase(roundDefinition.Id, cancellationToken);
        }
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

    private async Task SelectPowerPlays(string roundId, CancellationToken cancellationToken)
    {
        var roundState = GetRoundState(roundId);
        var playerIds = _playerIds.Select(x => x.ToString()).ToArray();
        var config = options.Value;
        var powerPlays = await new PowerPlaySelector(eventService)
            .Select(_gameId, playerIds, config.TimeForPowerPlaySelectionMs, cancellationToken);

        if (roundState is not null)
        {
            roundState.PowerPlays = powerPlays;
        }

        await _onStateUpdate(_state, cancellationToken);

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
        var config = options.Value;
        var questionDefinition = _definition.Rounds
            .FirstOrDefault(x => _state.CurrentRoundId == x.Id)
            ?.Categories.FirstOrDefault(x => x.Id == _state.CurrentCategoryId)
            ?.Questions.FirstOrDefault(x => x.Id == _state.CurrentQuestionId);

        var correctAnswerId = questionDefinition?.Answers.Where(x => x.IsCorrect).Select(x => x.Id).FirstOrDefault();
        var playerIds = _playerIds.Select(x => x.ToString()).ToArray();

        var answers = await new AnswerSelector(
            eventService, 
            correctAnswerId ?? string.Empty, 
            config.MaxPointsForAnswer, 
            config.MinPointsForAnswer, 
            config.PointsDecrement)
            .Select(_gameId, playerIds, config.TimeForAnswerSelectionMs, cancellationToken);

        foreach (var ans in answers)
        {
            await _onPlayerScoreUpdate(ans.PlayerId, ans.Points, cancellationToken);
        }

        var roundState = GetRoundState(roundId);
        if (roundState is not null)
        {
            roundState.Answers = answers;
            await _onStateUpdate(_state, cancellationToken);
        }
    }

    private async Task SelectCategory(string roundId, CancellationToken cancellationToken)
    {
        var gameId = _miniGameInstance.GameId.ToString();
        var roundState = GetRoundState(roundId);
        var roundDefinition = GetRoundDefinition(roundId);
        var config = options.Value;
        var categoryIds = roundDefinition?.Categories.Select(x => x.Id).ToList() ?? [];
        var playerIds = _playerIds.Select(x => x.ToString()).ToArray();
        var categories = await new CategorySelector(eventService, categoryIds)
            .Select(gameId, playerIds, config.TimeForCategorySelectionMs, cancellationToken);

        var selectedCategory = categories.Select(x => new { x.CategoryId, x.PlayerIds.Count })
            .OrderByDescending(x => x.Count)
            .GroupBy(x => x.Count)
            .FirstOrDefault()
            ?.FirstOrDefault();


        var category = roundDefinition?.Categories.FirstOrDefault(x => x.Id == selectedCategory?.CategoryId);
        var question = category?.Questions.FirstOrDefault();

        _state.CurrentCategoryId = category?.Id;
        _state.CurrentQuestionId = question?.Id;

        var round = _state.Rounds.FirstOrDefault(x => x.RoundId == roundId);
        if (round is not null && category is not null)
        {
            round.CategoryId = category.Id;
        }

        await _onStateUpdate(_state, cancellationToken);
    }
}