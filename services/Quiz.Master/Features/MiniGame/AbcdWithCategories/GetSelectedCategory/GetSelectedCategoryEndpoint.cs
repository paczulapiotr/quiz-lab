using Quiz.Common.CQRS;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetSelectedCategory;

public static partial class Endpoints
{
    public static void MapGetSelectedCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/game/{gameId}/mini-game/abcd/selected-category", async (Guid gameId, IQueryHandler<GetSelectedCategoryQuery, GetSelectedCategoryResult> commandHandler) =>
        {
            var Game = await commandHandler.HandleAsync(new GetSelectedCategoryQuery(gameId));

            return Results.Ok(Game);
        })
        .WithName("GetSelectedCategory")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Selected Category")
        .WithDescription("Get Selected Category")
        .WithTags("AbcdWithCategories");
    }
}