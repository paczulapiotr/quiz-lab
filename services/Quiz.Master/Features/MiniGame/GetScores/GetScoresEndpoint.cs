using Carter;
using Microsoft.AspNetCore.Mvc;
using Quiz.Common.CQRS;
using Quiz.Master.Extensions;

namespace Quiz.Master.Features.MiniGame.GetMiniGame;

public class GetScoresEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/game/{gameId}/mini-game/score", async ([FromRoute] Guid gameId, IHttpContextAccessor httpContextAccessor, IQueryHandler<GetScoresQuery, GetScoresResult> commandHandler) =>
        {
            var deviceId = httpContextAccessor.GetDeviceId();
            var Game = await commandHandler.HandleAsync(new GetScoresQuery(gameId, deviceId));

            return Results.Ok(Game);
        })
        .WithName("GetScores")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Scores")
        .WithDescription("Get Scores")
        .WithTags("MiniGame");
    }
}