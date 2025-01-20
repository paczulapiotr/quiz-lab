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
        await eventService.Initialize(miniGame.Id.ToString(), cancellationToken);
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

        foreach (var question in category.Questions) {

            await eventService.SendOnQuestionSelection(gameId, cancellationToken);

            await AnswerQuestion(question, cancellationToken);

            await eventService.SendOnQuestionAnswersPresentation(gameId, cancellationToken);

            await eventService.WaitForQuestionAnswersPresented(gameId, cancellationToken);
        }
    }

    private async Task<Definition.Category> SelectCategory(Definition.Round round, CancellationToken cancellationToken)
    {
        var gameId = _miniGame.GameId.ToString();
        var playerIds = _miniGame.PlayerIds.Select(x => x.ToString()).ToArray();
        var categoryIds = round.Categories.Select(x => x.Id).ToArray();
        var categories = await new CategorySelector(eventService, categoryIds)
            .Select(gameId, playerIds, options.Value.TimeForCategorySelectionMs, cancellationToken);

        var selectedCategoryId = categories.Select(x => new { x.CategoryId, x.PlayerIds.Count })
            .OrderByDescending(x => x.Count)
            .GroupBy(x => x.Count)
            .FirstOrDefault()
            ?.FirstOrDefault();

        var roundState = _state.Rounds.FirstOrDefault(x => x.RoundId == round.Id);

        if (roundState != null && selectedCategoryId != null)
        {
            roundState.CategoryId = selectedCategoryId.CategoryId;
        }
        await _onStateUpdate(_state, cancellationToken);

        var categoryDefinitions = round.Categories;
        return categoryDefinitions.FirstOrDefault(x => x.Id == selectedCategoryId?.CategoryId) ?? categoryDefinitions.First();
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

        var answers = await selector.Select(gameId, playerIds, config.TimeForAnswerSelectionMs, cancellationToken);

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