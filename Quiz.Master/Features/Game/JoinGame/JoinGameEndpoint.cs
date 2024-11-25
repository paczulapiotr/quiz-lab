using Carter;
using Quiz.Common.CQRS;

namespace Quiz.Master.Features.Game.JoinGame;

public record JoinGameRequest(string DeviceId, string PlayerName);


public class JoinGameEndpoint() : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/game/join", async (JoinGameRequest request, ICommandHandler<JoinGameCommand> commandHandler) =>
        {
            await commandHandler.HandleAsync(new JoinGameCommand(request.DeviceId, request.PlayerName));
            return Results.Ok();
        })
        .WithName("JoinGame")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Join Game")
        .WithDescription("Join Game")
        .WithTags("Game");
    }
}