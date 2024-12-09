using Quiz.Common.CQRS;

namespace Quiz.Master.Features.Game.CreateGame;

public record CreateGameRequest(uint GameSize);

public static partial class Endpoints
{
    public static void MapCreateGame(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/game/create", async (CreateGameRequest request, ICommandHandler<CreateGameCommand> commandHandler) =>
        {
            await commandHandler.HandleAsync(new CreateGameCommand(request.GameSize));

            return Results.Created();
        })
        .WithName("CreateGame")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Game")
        .WithDescription("Create Game")
        .WithTags("Game");
    }
}