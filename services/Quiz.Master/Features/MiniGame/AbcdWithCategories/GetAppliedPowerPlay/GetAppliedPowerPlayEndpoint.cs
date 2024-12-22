using Quiz.Common.CQRS;
using Quiz.Master.Extensions;
using Quiz.Master.Features.MiniGame.AbcdWithCategories.GetPowerPlays;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetAppliedPowerPlay;

public static partial class Endpoints
{
    public static void MapGetAppliedPowerPlayEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/game/{gameId}/mini-game/abcd/applied-power-play", async (Guid gameId, IHttpContextAccessor httpContextAccessor, IQueryHandler<GetPowerPlaysQuery, GetPowerPlaysResult> commandHandler) =>
        {
            var deviceId = httpContextAccessor.GetDeviceId();
            var result = await commandHandler.HandleAsync(new GetPowerPlaysQuery(gameId, deviceId));

            return Results.Ok(result);
        })
        .WithName("GetAppliedPower")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Applied Power")
        .WithDescription("Get Applied Power")
        .WithTags("AbcdWithCategories");
    }
}