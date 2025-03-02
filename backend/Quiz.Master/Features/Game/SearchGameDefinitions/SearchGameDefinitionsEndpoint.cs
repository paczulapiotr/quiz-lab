using Carter;

namespace Quiz.Master.Features.MiniGame.SearchGameDefinitions;

public class SearchGameDefinitions : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/game/definitions", async (IContentManagementClient client) =>
        {
            var names = await client.GetGameNames();

            return Results.Ok(names);
        })
        .WithName("SearchGameDefinitions")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Search Game Definitions")
        .WithDescription("Search Game Definitions")
        .WithTags("Game");
    }
}