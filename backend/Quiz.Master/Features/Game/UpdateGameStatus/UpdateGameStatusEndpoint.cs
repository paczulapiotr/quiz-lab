using Carter;
using Microsoft.AspNetCore.Mvc;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.Game;

namespace Quiz.Master.Features.MiniGame.UpdateGameStatus;

public class UpdateGameStatusEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/game/{gameId}/status", async (
            [FromRoute] string gameId,
            [FromBody] UpdateGameStatusRequest request,
            IPublisher publisher) =>
        {
            await publisher.PublishAsync(
                new GameStatusUpdate(gameId, request.Status), 
                request.Status == GameStatus.GameStarted 
                    ? "new" 
                    : gameId);

            return Results.NoContent();
        })
        .WithName("UpdateGameStatus")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Game Status")
        .WithDescription("Update Game Status")
        .WithTags("Game");
    }
}

public record UpdateGameStatusRequest(GameStatus Status);