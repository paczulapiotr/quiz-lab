using Quiz.Common.CQRS;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetQuestion;

public static partial class Endpoints
{
    public static void MapGetQuestionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/game/{gameId}/mini-game/abcd/question", async (Guid gameId, IQueryHandler<GetQuestionQuery, GetQuestionResult> commandHandler) =>
        {
            var Game = await commandHandler.HandleAsync(new GetQuestionQuery(gameId));

            return Results.Ok(Game);
        })
        .WithName("GetQuestion")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Question")
        .WithDescription("Get Question")
        .WithTags("AbcdWithCategories");
    }
}