using Carter;
using Quiz.Master.Persistance.Repositories.Abstract;

namespace Quiz.Master.Features.Device.GetDeviceId;

public record Response(string? DeviceId, string? HostId, string? RoomCode, string? GameId);

public class GetDeviceIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/device", async (IGameRepository repository, IHttpContextAccessor httpContextAccessor, CancellationToken token = default) =>
        {
            var ctx = httpContextAccessor.HttpContext!;
            var deviceId = ctx.Request.Headers["deviceId"].ToString();
            var hostId = ctx.Request.Headers["hostId"].ToString();
            var roomCode = ctx.Request.Headers["roomCode"].ToString();

            var room = roomCode != null ? await repository.FindRoomByCodeAsync(roomCode, token) : null;

            roomCode = room?.Code;
            deviceId = room?.PlayerDeviceIds.FirstOrDefault(x => x == deviceId);
            hostId = room?.HostDeviceId == hostId ? hostId : null;
            var gameId = room?.GameId?.ToString();

            return Results.Ok(new Response(deviceId, hostId, roomCode, gameId));
        })
         .WithName("GetDeviceId")
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithSummary("Get Device Id")
         .WithDescription("Get Device Id")
         .WithTags("Device");
    }
}