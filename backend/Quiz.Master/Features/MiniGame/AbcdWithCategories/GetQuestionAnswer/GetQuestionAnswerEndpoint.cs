using Carter;
using Quiz.Common.CQRS;
using Quiz.Master.Extensions;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetQuestionAnswer;

public class GetQuestionAnswerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/game/{gameId}/mini-game/abcd/question-answer", async (Guid gameId, IHttpContextAccessor httpContextAccessor, IQueryHandler<GetQuestionAnswerQuery, GetQuestionAnswerResult> commandHandler) =>
        {
            var deviceId = httpContextAccessor.GetDeviceId();
            var result = await commandHandler.HandleAsync(new GetQuestionAnswerQuery(gameId, deviceId));

            return Results.Ok(result);
        })
        .WithName("GetQuestionAnswer")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Question Answer")
        .WithDescription("Get Question Answer")
        .WithTags("AbcdWithCategories");
    }
}