using Microsoft.AspNetCore.Mvc;
using Quiz.Common.CQRS;

namespace Quiz.Master.Features.Game.GetGame;


public static partial class Endpoints
{
    public static void MapGetMiniGame(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/game/{gameId}/mini-game", async ([FromRoute] Guid gameId, IQueryHandler<GetMiniGameQuery, GetMiniGameResult> commandHandler) =>
        {
            var Game = await commandHandler.HandleAsync(new GetMiniGameQuery(gameId));

            return Results.Ok(Game);
        })
        .WithName("GetMiniGame")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Mini Game")
        .WithDescription("Get Mini Game")
        .WithTags("MiniGame");
    }
}