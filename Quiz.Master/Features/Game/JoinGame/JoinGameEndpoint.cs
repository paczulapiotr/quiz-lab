using Quiz.Common.CQRS;
using Quiz.Master.Extensions;

namespace Quiz.Master.Features.Game.JoinGame;

public record JoinGameRequest(string PlayerName);

public static partial class Endpoints
{
    public static void MapJoinGame(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/game/join", async (JoinGameRequest request, ICommandHandler<JoinGameCommand> commandHandler, IHttpContextAccessor httpContextAccessor) =>
        {
            await commandHandler.HandleAsync(new JoinGameCommand(httpContextAccessor.GetDeviceId(), request.PlayerName));
            return Results.Ok();
        })
        .WithName("JoinGame")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Join Game")
        .WithDescription("Join Game")
        .WithTags("Game");
    }
}