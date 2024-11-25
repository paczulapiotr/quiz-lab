using Carter;
using Quiz.Common.CQRS;

namespace Quiz.Master.Features.Game.JoinGame;

public class GetCurrentGameEndpoint() : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/game/current", async (IQueryHandler<GetCurrentGameQuery, GetCurrentGameResult> commandHandler) =>
        {
            var currentGame = await commandHandler.HandleAsync(new GetCurrentGameQuery());

            return Results.Ok(currentGame);
        })
        .WithName("GetCurrentGame")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Current Game")
        .WithDescription("Get Current Game")
        .WithTags("Game");
    }
}