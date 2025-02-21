using Carter;
using Microsoft.AspNetCore.Mvc;
using Quiz.Common.CQRS;
using Quiz.Master.Extensions;

namespace Quiz.Master.Features.MiniGame.SendPlayerInteraction;

public class SendPlayerInteractionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/game/{gameId}/mini-game/interaction", async (
            [FromRoute] Guid gameId,
            [FromBody] SendPlayerInteractionRequest request,
            IHttpContextAccessor httpContextAccessor,
            ICommandHandler<SendPlayerInteractionCommand> commandHandler) =>
        {

            var deviceId = httpContextAccessor.GetUniqueId()!;
            var Game = await commandHandler.HandleAsync(new SendPlayerInteractionCommand(
                gameId,
                deviceId,
                request.InteractionType,
                request.Value,
                request.Data));

            return Results.Ok(Game);
        })
        .WithName("SendPlayerInteraction")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Send Player Interaction")
        .WithDescription("Send Player Interaction")
        .WithTags("MiniGame");
    }
}

public record SendPlayerInteractionRequest(string InteractionType, string Value, Dictionary<string, string>? Data);