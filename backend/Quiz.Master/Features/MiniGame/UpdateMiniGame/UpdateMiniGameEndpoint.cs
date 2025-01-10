using Carter;
using Microsoft.AspNetCore.Mvc;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;

namespace Quiz.Master.Features.MiniGame.UpdateMiniGame;

public class UpdateMiniGameEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/game/{gameId}/mini-game/update", async (
            [FromRoute] string gameId,
            [FromBody] UpdateMiniGameEndpointRequest request,
            IPublisher publisher) =>
        {
            await publisher.PublishAsync(new MiniGameUpdate(gameId, request.Action), gameId);

            return Results.NoContent();
        })
        .WithName("UpdateMiniGame")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Mini Game")
        .WithDescription("Update Mini Game")
        .WithTags("MiniGame");
    }
}

public record UpdateMiniGameEndpointRequest(string Action);