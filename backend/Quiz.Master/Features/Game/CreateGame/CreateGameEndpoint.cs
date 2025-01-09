using Carter;
using Quiz.Common.CQRS;

namespace Quiz.Master.Features.Game.CreateGame;

public record CreateGameRequest(uint GameSize, Guid GameDefinitionId);

public class CreateGameEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/game/create", async (CreateGameRequest request, ICommandHandler<CreateGameCommand> commandHandler) =>
        {
            await commandHandler.HandleAsync(new CreateGameCommand(request.GameSize, request.GameDefinitionId));

            return Results.Created();
        })
        .WithName("CreateGame")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Game")
        .WithDescription("Create Game")
        .WithTags("Game");
    }
}