using System.Text.Json;
using Quiz.Common.Broker.Consumer;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;
using Quiz.Master.Persistance.Models;
using Quiz.Master.Persistance.Models.MiniGames.AbcdCategories;
using Quiz.Master.Persistance.Repositories.Abstract;

namespace Quiz.Master.Game.MiniGames;

public class AbcdWithCategoriesMiniGame(
    IOneTimeConsumer<PlayerInteraction> playerInteracton,
    IOneTimeConsumer<MiniGameUpdate> miniGameUpdate,
    IPublisher publisher,
    IMiniGameSaveRepository miniGameSaveRepository) : IMiniGameHandler
{
    private static string MiniGameType => Persistance.Models.MiniGameType.AbcdWithCategories.ToString();
    public async Task HandleMiniGame(MiniGameInstance game, CancellationToken cancellationToken = default)
    {
        var miniGameData = GetJsonData<AbcdWithCategories>(game);

        if (miniGameData is null)
        {
            throw new InvalidOperationException("Invalid mini game configuration");
        }
        var config = miniGameData.Config;
        var firstRound = miniGameData.Rounds.FirstOrDefault();
        if (firstRound is null)
        {
            throw new InvalidOperationException("Invalid mini game configuration - no rounds found");
        }

        var gameId = game.GameId.ToString();
        await RunRoundBase(game, miniGameData, firstRound, cancellationToken);

        // RABBIT_SEND start explaining PowerPlay
        await publisher.PublishAsync(new MiniGameNotification(gameId, MiniGameType, Action: "PowerPlayExplainStart"), cancellationToken);
        // RABBIT_RECEIVE stop explaining PowerPlay
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == "PowerPlayExplainStop", cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, MiniGameType, Action: "PowerPlayExplainStop"), cancellationToken);

        // For each question after first one
        foreach (var round in miniGameData.Rounds.Skip(1))
        {
            // Save PowerPlay selection for every player
            await SelectPowerPlays(game, miniGameData, round, cancellationToken);

            // USE Round_Base
            await RunRoundBase(game, miniGameData, round, cancellationToken);
        }
    }

    private async Task SelectPowerPlays(MiniGameInstance game, AbcdWithCategories miniGameData, AbcdWithCategories.Round round, CancellationToken cancellationToken)
    {
        var gameId = game.GameId.ToString();
        // RABBIT_SEND start selecting PowerPlay
        await publisher.PublishAsync(new MiniGameNotification(gameId, MiniGameType, Action: "PowerPlayStart"), cancellationToken);
        // HTTP_RECEIVE Receive ZAGRYWKA selection or wait for time to pass

        var timeToken = new CancellationTokenSource(miniGameData.Config.TimeForPowerPlaySelectionMs).Token;
        // <playerId, (PowerPlay, sourcePlayerId)[]>
        var powerPlays = new PowerPlaysDictionary();
        var categoryIds = round.Categories.Select(x => x.Id).ToList();
        var playersCount = game.Game.PlayersCount;
        for (int i = 0; i < playersCount; i++)
        {
            var playersThatSelected = powerPlays.Values.SelectMany(x => x.Select(x => x.sourcePlayerId)).Distinct().ToArray();
            var selection = await playerInteracton.ConsumeFirstAsync(
                condition: x => x.GameId == gameId
                    && !playersThatSelected.Contains(x.PlayerId)
                    && x.InteractionType == "PowerPlaySelection"
                    && x.Value != null,
                cancellationToken: timeToken);

            if (timeToken.IsCancellationRequested)
            {
                break;
            }
            var actionData = selection!.Data!;
            var playerId = actionData["playerId"]!;
            var powerPlay = Enum.Parse<AbcdWithCategories.PowerPlay>(actionData["powerPlay"]);

            if (powerPlays.ContainsKey(playerId))
            {
                powerPlays[playerId].Append((powerPlay, selection.PlayerId));
            }
            else
            {
                powerPlays.Add(playerId, [(powerPlay, selection.PlayerId)]);
            }
        }
        var roundState = miniGameData.State.Rounds.First(x => x.RoundId == round.Id);
        roundState.PowerPlays = powerPlays;

        // RABBIT_SEND start showing PowerPlays
        await publisher.PublishAsync(new MiniGameNotification(gameId, MiniGameType, Action: "PowerPlayApplyStart"), cancellationToken);
        // RABBIT_RECEIVE stop showing PowerPlays
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == "PowerPlayApplyStop", cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, MiniGameType, Action: "PowerPlayApplyStop"), cancellationToken);
    }

    private async Task RunRoundBase(MiniGameInstance game, AbcdWithCategories miniGameData, AbcdWithCategories.Round round, CancellationToken cancellationToken)
    {
        var gameId = game.GameId.ToString();
        var playersCount = game.Game.PlayersCount;
        var config = miniGameData!.Config;
        miniGameData.State.Rounds.Add(new()
        {
            RoundId = round!.Id,
        });

        await SaveState(game, miniGameData, cancellationToken);

        // Choose most voted category or random if tied
        var selectedCategoryId = await SelectCategoryId(gameId, round, playersCount, config.TimeForCategorySelectionMs);
        var selectedCategory = round.Categories.FirstOrDefault(x => x.Id == selectedCategoryId);
        miniGameData.State.Rounds[0].CategoryId = selectedCategoryId;
        await SaveState(game, miniGameData, cancellationToken);

        // RABBIT_SEND start showing selected category
        await publisher.PublishAsync(new MiniGameNotification(gameId, MiniGameType, Action: "CategoryShowStart"), cancellationToken);

        // RABBIT_RECEIVE stop showing selected category
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == "CategoryShowStop", cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, MiniGameType, "CategoryShowStop"), cancellationToken);

        // RABBIT_SEND start showing question
        await publisher.PublishAsync(new MiniGameNotification(gameId, MiniGameType, Action: "QuestionShowStart"), cancellationToken);

        // RABBIT_RECEIVE stop showing question
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == "QuestionShowStop", cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, MiniGameType, "QuestionShowStop"), cancellationToken);

        // RABBIT_SEND start countdown for answers
        await publisher.PublishAsync(new MiniGameNotification(gameId, MiniGameType, Action: "QuestionAnswerStart"), cancellationToken);

        // HTTP_RECEIVE Wait for answers or wait for time to pass
        await AnswerQuestion(game, miniGameData, selectedCategory!);
        await SaveState(game, miniGameData, cancellationToken);

        // RABBIT_SEND start showing answers
        await publisher.PublishAsync(new MiniGameNotification(gameId, MiniGameType, Action: "QuestionAnswerShowStart"), cancellationToken);

        // RABBIT_RECEIVE stop showing answers
        await miniGameUpdate.ConsumeFirstAsync(condition: x => x.Action == "QuestionAnswerShowStop", cancellationToken: cancellationToken);
        await publisher.PublishAsync(new MiniGameNotification(gameId, MiniGameType, "QuestionAnswerShowStop"), cancellationToken);
    }

    private async Task AnswerQuestion(MiniGameInstance game, AbcdWithCategories miniGameData, AbcdWithCategories.Category selectedCategory)
    {
        var gameId = game.GameId.ToString();
        var playersCount = game.Game.PlayersCount;
        var timeToken = new CancellationTokenSource(miniGameData.Config.TimeForAnswerSelectionMs).Token;
        var playerIds = game.Game.Players.Select(x => x.Id.ToString()).ToList();
        // <playerId, (answerId, timestamp)>
        var answers = new Dictionary<string, (string answerId, DateTime timestamp)>();

        for (int i = 0; i < playersCount; i++)
        {
            var answer = await playerInteracton.ConsumeFirstAsync(
                condition: x => x.GameId == gameId
                    && playerIds.Contains(x.PlayerId)
                    && !answers.Keys.Contains(x.PlayerId)
                    && x.InteractionType == "QuestionAnswer"
                    && x.Value != null,
            cancellationToken: timeToken);

            if (timeToken.IsCancellationRequested)
            {
                break;
            }

            answers.TryAdd(answer!.PlayerId, (answer.Value!, answer.Timestamp));
        }
        var correctAnswerIds = selectedCategory!.Questions.First().Answers.Where(x => x.IsCorrect).Select(x => x.Id).ToList();
        var correctAnswers = answers
            .Where(x => correctAnswerIds.Contains(x.Value.answerId))
            .OrderByDescending(x => x.Value.timestamp)
            .ToArray();

        for (int i = 0; i < correctAnswers.Count(); i++)
        {
            var ans = correctAnswers[i]!;
            var score = game.PlayerScores.FirstOrDefault(x => x.PlayerId.ToString() == ans.Key);
            if (score is null)
            {
                score = new MiniGameInstanceScore
                {
                    Score = 0,
                    PlayerId = Guid.Parse(ans.Key),
                    MiniGameInstanceId = game.Id,
                };
                game.PlayerScores.Append(score);
            }

            score.Score += Math.Max(miniGameData.Config.MaxPointsForAnswer - i * miniGameData.Config.PointsDecrement, miniGameData.Config.MinPointsForAnswer);
        }
    }

    private async Task<string> SelectCategoryId(string gameId, AbcdWithCategories.Round firstRound, int playersCount, int timeForCategorySelectionMs)
    {
        var timeToken = new CancellationTokenSource(timeForCategorySelectionMs).Token;

        // <categoryId, playerId[]>
        var selections = new Dictionary<string, string[]>();
        var categoryIds = firstRound.Categories.Select(x => x.Id).ToList();
        for (int i = 0; i < playersCount; i++)
        {
            var playersThatSelected = selections.Values.SelectMany(x => x).Distinct().ToArray();
            var selection = await playerInteracton.ConsumeFirstAsync(
                condition: x => x.GameId == gameId
                    && !playersThatSelected.Contains(x.PlayerId)
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
                selections[selection!.Value!].Append(selection!.PlayerId);
            }
            else
            {
                selections.Add(selection!.Value!, [selection!.PlayerId]);
            }

        }

        var selectedCategoryId = selections.OrderByDescending(x => x.Value.Length).FirstOrDefault().Key
            ?? categoryIds[new Random().Next(categoryIds.Count)];

        return selectedCategoryId;
    }


    private async Task SaveState(MiniGameInstance game, AbcdWithCategories state, CancellationToken cancellationToken)
    {
        await miniGameSaveRepository.SaveMiniGame<
            AbcdWithCategories,
            AbcdWithCategories.Configuration,
            AbcdWithCategories.MiniGameState>
            (game, state, cancellationToken);
    }

    private TData? GetJsonData<TData>(MiniGameInstance game) => JsonSerializer.Deserialize<TData>(game.RoundsJsonData);
}