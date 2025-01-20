using Carter;
using Quiz.Master.MiniGames.Models.MusicGuess;
using Quiz.Storage;

namespace Quiz.Master.Features.MiniGame.MusicGuess.GetSelectedCategory;

public class GetSelectedCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/game/{gameId}/mini-game/music/selected-category", async (Guid gameId, IDatabaseStorage storage, CancellationToken cancellationToken = default) =>
        {
            var game = await storage.FindGameAsync(gameId, cancellationToken);

            if (game.CurrentMiniGameId is null)
            {
                throw new InvalidOperationException("Game has no current mini game");
            }

            var miniGame = await storage.FindMiniGameAsync(game.CurrentMiniGameId.Value);
            var miniGameDefinition = await storage.FindMiniGameDefinitionAsync(miniGame.MiniGameDefinitionId);

            var state = miniGame.State.As<MusicGuessState>();
            var definition = miniGameDefinition.Definition.As<MusicGuessDefinition>();

            var categories = definition?.Rounds?.FirstOrDefault(x => x.Id == state?.CurrentRoundId)
                ?.Categories ?? [];

            var players = game.Players;

            var selectedCategories = categories.Select(c => new SelectedCategory(
                c.Id,
                c.Name,
                c.Id == state?.CurrentCategoryId,
                state?.Rounds?.FirstOrDefault(x => x.RoundId == state?.CurrentRoundId)
                    ?.SelectedCategories.FirstOrDefault(x => x.CategoryId == c.Id)?.PlayerIds
                    .Select(playerId => new Player(playerId.ToString(), players?.FirstOrDefault(p => p.Id == playerId)?.Name ?? "")).ToArray() ?? []
            )).ToArray();

            return Results.Ok(null);
        })
        .WithName("GetSelectedCategory")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Selected Category")
        .WithDescription("Get Selected Category")
        .WithTags("MusicGuess");
    }
}

internal record GetSelectedCategoryResult(SelectedCategory[] Categories);
internal record SelectedCategory(string Id, string Text, bool IsSelected, Player[] Players);
internal record Player(string Id, string Name);