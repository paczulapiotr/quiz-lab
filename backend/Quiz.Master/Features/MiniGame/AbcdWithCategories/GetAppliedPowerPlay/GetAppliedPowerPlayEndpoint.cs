using Carter;
using Quiz.Common.CQRS;
using Quiz.Master.Extensions;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetAppliedPowerPlay;

public class GetAppliedPowerPlayEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/game/{gameId}/mini-game/abcd/applied-power-play", async (Guid gameId, IHttpContextAccessor httpContextAccessor, IQueryHandler<GetAppliedPowerPlayQuery, GetAppliedPowerPlayResult> commandHandler) =>
        {
            var deviceId = httpContextAccessor.GetDeviceId();
            var result = await commandHandler.HandleAsync(new GetAppliedPowerPlayQuery(gameId, deviceId));

            return Results.Ok(result);
        })
        .WithName("GetAppliedPower")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Applied Power")
        .WithDescription("Get Applied Power")
        .WithTags("AbcdWithCategories");
    }
}