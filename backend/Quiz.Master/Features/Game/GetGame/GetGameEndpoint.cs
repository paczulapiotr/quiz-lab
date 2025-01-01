using Carter;
using Quiz.Common.CQRS;

namespace Quiz.Master.Features.Game.GetGame;

public class GetGameEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/game/{gameId}", async (Guid gameId, IQueryHandler<GetGameQuery, GetGameResult> commandHandler) =>
        {
            var Game = await commandHandler.HandleAsync(new GetGameQuery(gameId));

            return Results.Ok(Game);
        })
        .WithName("GetGame")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Game")
        .WithDescription("Get Game")
        .WithTags("Game");
    }
}