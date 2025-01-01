using Carter;
using Quiz.Common.CQRS;
using Quiz.Master.Extensions;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetPowerPlays;

public class GetPowerPlaysEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/game/{gameId}/mini-game/abcd/power-plays", async (Guid gameId, IHttpContextAccessor httpContextAccessor, IQueryHandler<GetPowerPlaysQuery, GetPowerPlaysResult> commandHandler) =>
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