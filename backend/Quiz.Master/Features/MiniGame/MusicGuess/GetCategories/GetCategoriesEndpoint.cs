using Carter;
using Quiz.Master.MiniGames.Models.MusicGuess;
using Quiz.Storage;

namespace Quiz.Master.Features.MiniGame.MusicGuess.GetCategories;

public class GetCategoriesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/game/{gameId}/mini-game/music/categories", async (Guid gameId, IDatabaseStorage storage, CancellationToken cancellationToken = default) =>
        {
            var (state, definition) = await storage.FindCurrentMiniGameStateAndDefinitionAsync
                <MusicGuessState, MusicGuessDefinition>(gameId, cancellationToken);

            var categories = definition?.Rounds?.FirstOrDefault(x => x.Id == state?.CurrentRoundId)
                ?.Categories;

            if (categories is null)
            {
                throw new InvalidOperationException("Question not found");
            }
            
            var result = new GetCategoriesResult(categories.Select(x => new Category(x.Id, x.Name)));

            return Results.Ok(result);
        })
        .WithName("MusicGetCategories")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Categories")
        .WithDescription("Get Categories")
        .WithTags("MusicGuess");
    }
}

internal record GetCategoriesResult(IEnumerable<Category> Categories);
internal record Category(string Id, string Text);