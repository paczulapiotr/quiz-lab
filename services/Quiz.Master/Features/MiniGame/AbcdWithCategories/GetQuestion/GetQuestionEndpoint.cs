using Carter;
using Quiz.Common.CQRS;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetQuestion;

public class GetQuestionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/game/{gameId}/mini-game/abcd/question", async (Guid gameId, IQueryHandler<GetQuestionQuery, GetQuestionResult> commandHandler) =>
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