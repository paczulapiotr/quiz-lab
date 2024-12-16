using Microsoft.AspNetCore.Mvc;
using Quiz.Common.CQRS;
using Quiz.Master.Extensions;

namespace Quiz.Master.Features.MiniGame.GetMiniGame;

public static partial class Endpoints
{
    public static void MapGetMiniGame(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/game/{gameId}/mini-game", async ([FromRoute] Guid gameId, IHttpContextAccessor httpContextAccessor, IQueryHandler<GetMiniGameQuery, GetMiniGameResult> commandHandler) =>
        {
            var deviceId = httpContextAccessor.GetDeviceId();
            var Game = await commandHandler.HandleAsync(new GetMiniGameQuery(gameId, deviceId));

            return Results.Ok(Game);
        })
        .WithName("GetMiniGame")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Mini Game")
        .WithDescription("Get Mini Game")
        .WithTags("MiniGame");
    }
}