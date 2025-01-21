using Carter;
using Quiz.Common.CQRS;

namespace Quiz.Master.Features.Game.JoinGame;

public record JoinGameRequestDto(string PlayerName, Guid GameId);

public class JoinGameEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/game/join", async (JoinGameRequestDto request, IRequestHandler<JoinGameRequest, JoinGameResult> requestHandler, IHttpContextAccessor httpContextAccessor) =>
         {
             var result = await requestHandler.HandleAsync(new JoinGameRequest(request.GameId, request.PlayerName));
             return Results.Ok(result);
         })
         .WithName("JoinGame")
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithSummary("Join Game")
         .WithDescription("Join Game")
         .WithTags("Game");
    }
}