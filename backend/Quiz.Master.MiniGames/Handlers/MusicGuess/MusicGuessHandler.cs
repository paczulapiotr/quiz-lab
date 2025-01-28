using Microsoft.Extensions.Options;
using Quiz.Master.Game.MiniGames;
using State = Quiz.Master.MiniGames.Models.MusicGuess.MusicGuessState;
using Definition = Quiz.Master.MiniGames.Models.MusicGuess.MusicGuessDefinition;
using Configuration = Quiz.Master.MiniGames.Models.MusicGuess.Configuration;
using Quiz.Master.MiniGames.Handlers.MusicGuess.Logic;

namespace Quiz.Master.MiniGames.Handlers.MusicGuess;

public class MusicGuessHandler(IMiniGameEventService eventService, IMiniGameRepository repository, IOptions<Configuration> options) : IMiniGameHandler
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
        if(definition == null) {
            throw new InvalidOperationException("Mini game definition not found");
        }
        await eventService.Initialize(miniGame.GameId.ToString(), cancellationToken);
        foreach (var round in definition.Rounds) {
            var roundState = new State.RoundState { RoundId = round.Id };
            _state.Rounds.Add(roundState);

            await RunRoundBase(round, cancellationToken);
        }
    }

    private async Task RunRoundBase(Definition.Round round, CancellationToken cancellationToken)
    {
        var gameId = _miniGame.GameId.ToString();
        _state.CurrentRoundId = round.Id;
        await _onStateUpdate(_state, cancellationToken);

        await eventService.SendOnCategorySelection(gameId, cancellationToken);

        // Choose most voted category or random iftied
        var category = await SelectCategory(round, cancellationToken);

        await eventService.SendOnCategorySelected(gameId, cancellationToken);

        await eventService.WaitForCategoryPresented(gameId, cancellationToken);

        foreach (var question in category.Questions)
        {
            await SetCurrentQuestion(round, question, cancellationToken);

            await eventService.SendOnQuestionSelection(gameId, cancellationToken);

            await AnswerQuestion(question, cancellationToken);

            await eventService.SendOnQuestionAnswersPresentation(gameId, cancellationToken);

            await eventService.WaitForQuestionAnswersPresented(gameId, cancellationToken);
        }
    }

    private async Task SetCurrentQuestion(Definition.Round round, Definition.Question question, CancellationToken cancellationToken)
    {
        _state.CurrentQuestionId = round.Categories
            .FirstOrDefault(x => x.Id == _state.CurrentCategoryId)
            ?.Questions
            .FirstOrDefault(x => x.Id == question.Id)?.Id;
            
        await _onStateUpdate(_state, cancellationToken);
    }

    private async Task<Definition.Category> SelectCategory(Definition.Round round, CancellationToken cancellationToken)
    {
        var gameId = _miniGame.GameId.ToString();
        var playerIds = _miniGame.PlayerIds.Select(x => x.ToString()).ToArray();
        var categoryIds = round.Categories.Select(x => x.Id).ToArray();
        var initialState = categoryIds.Select(x => new State.SelectedCategory { CategoryId = x, PlayerIds = [] }).ToList();
        var categories = await new CategorySelector(eventService, categoryIds)
            .Select(gameId, initialState, new(options.Value.TimeForCategorySelectionMs, playerIds), cancellationToken);

        var selectedCategory = categories?
            .OrderByDescending(x => x.PlayerIds.Count)
            .GroupBy(x => x.PlayerIds.Count)
            .FirstOrDefault()
            ?.FirstOrDefault();

        var roundState = _state.Rounds.FirstOrDefault(x => x.RoundId == round.Id);

        if (roundState != null && selectedCategory != null)
        {
            _state.CurrentCategoryId = selectedCategory.CategoryId;
            roundState.CategoryId = selectedCategory.CategoryId;
            roundState.SelectedCategories = categories ?? [];
        }
        await _onStateUpdate(_state, cancellationToken);

        var categoryDefinitions = round.Categories;
        return categoryDefinitions.FirstOrDefault(x => x.Id == selectedCategory?.CategoryId) ?? categoryDefinitions.First();
    }

    private async Task AnswerQuestion(Definition.Question question, CancellationToken cancellationToken)
    {
        var gameId = _miniGame.GameId.ToString();
        var playerIds = _miniGame.PlayerIds.Select(x => x.ToString()).ToArray();
        var answerIds = question.Answers.Select(x => x.Id).ToArray();
        var correctAnswerId = question.Answers.First(x => x.IsCorrect).Id;
        var config = options.Value;
        var selector = new AnswerSelector(
            eventService, 
            answerIds, 
            correctAnswerId,
            config.MaxPointsForAnswer,
            config.MinPointsForAnswer,
            config.PointsDecrement);

        var answers = await selector.Select(gameId, new(), new(config.TimeForAnswerSelectionMs, playerIds), cancellationToken) ?? [];

        foreach (var ans in answers)
        {
            await _onPlayerScoreUpdate(ans.PlayerId, ans.Points, cancellationToken);
        }

        var round = _state.Rounds.FirstOrDefault(x => x.RoundId == _state.CurrentRoundId);

        if(round != null) {
            round.Answers = answers;
        }

        await _onStateUpdate(_state, cancellationToken);
    }
}