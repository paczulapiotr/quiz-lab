using Quiz.Common.CQRS;
using Quiz.Master.Extensions;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetPowerPlays;

public static partial class Endpoints
{
    public static void MapGetPowerPlaysEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/game/{gameId}/mini-game/abcd/power-plays", async (Guid gameId, IHttpContextAccessor httpContextAccessor, IQueryHandler<GetPowerPlaysQuery, GetPowerPlaysResult> commandHandler) =>
        {
            var deviceId = httpContextAccessor.GetDeviceId();
            var Game = await commandHandler.HandleAsync(new GetPowerPlaysQuery(gameId, deviceId));

            return Results.Ok(Game);
        })
        .WithName("GetPowerPlays")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Power Plays")
        .WithDescription("Get Power Plays")
        .WithTags("AbcdWithCategories");
    }
}