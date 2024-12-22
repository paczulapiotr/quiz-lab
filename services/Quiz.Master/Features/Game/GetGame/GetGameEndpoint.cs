using Quiz.Common.CQRS;

namespace Quiz.Master.Features.Game.GetGame;

public static partial class Endpoints
{
    public static void MapGetGame(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/game/{gameId}", async (Guid gameId, IQueryHandler<GetGameQuery, GetGameResult> commandHandler) =>
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