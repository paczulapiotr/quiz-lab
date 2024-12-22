using Quiz.Common.CQRS;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetCategories;

public static partial class Endpoints
{
    public static void MapGetCategoriesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/game/{gameId}/mini-game/abcd/categories", async (Guid gameId, IQueryHandler<GetCategoriesQuery, GetCategoriesResult> commandHandler) =>
        {
            var Game = await commandHandler.HandleAsync(new GetCategoriesQuery(gameId));

            return Results.Ok(Game);
        })
        .WithName("GetCategories")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Categories")
        .WithDescription("Get Categories")
        .WithTags("AbcdWithCategories");
    }
}