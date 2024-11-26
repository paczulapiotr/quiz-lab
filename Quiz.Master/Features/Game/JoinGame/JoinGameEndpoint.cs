using Quiz.Common.CQRS;

namespace Quiz.Master.Features.Game.JoinGame;

public record JoinGameRequest(string PlayerName, Guid GameId);

public static partial class Endpoints
{
    public static void MapJoinGame(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/game/join", async (JoinGameRequest request, ICommandHandler<JoinGameCommand> commandHandler, IHttpContextAccessor httpContextAccessor) =>
        {
            await commandHandler.HandleAsync(new JoinGameCommand(request.GameId, request.PlayerName));
            return Results.Ok();
        })
        .WithName("JoinGame")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Join Game")
        .WithDescription("Join Game")
        .WithTags("Game");
    }
}