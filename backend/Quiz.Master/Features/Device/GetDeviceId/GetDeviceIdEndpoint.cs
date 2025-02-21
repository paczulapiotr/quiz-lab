using Carter;
using Quiz.Master.Persistance.Repositories.Abstract;

namespace Quiz.Master.Features.Device.GetDeviceId;

public record Response(string? DeviceId, string? HostId, string? RoomCode);

public class GetDeviceIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/device", async (IGameRepository repository, IHttpContextAccessor httpContextAccessor, CancellationToken token = default) =>
        {
            var ctx = httpContextAccessor.HttpContext!;
            var deviceId = ctx.Request.Cookies["deviceId"];
            var hostId = ctx.Request.Cookies["hostId"];
            var roomCode = ctx.Request.Cookies["roomCode"];

            var room = roomCode != null ? await repository.FindRoomByCodeAsync(roomCode, token) : null;

            roomCode = room?.Code;
            deviceId = room?.PlayerDeviceIds.FirstOrDefault(x => x == deviceId);
            hostId = room?.HostDeviceId == hostId ? hostId : null;

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            };

            UpdateCookie(ctx, "hostId", hostId, cookieOptions);
            UpdateCookie(ctx, "deviceId", deviceId, cookieOptions);
            UpdateCookie(ctx, "roomCode", roomCode, cookieOptions);

            return Results.Ok(new Response(deviceId, hostId, roomCode));
        })
                 .WithName("GetDeviceId")
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithSummary("Get Device Id")
         .WithDescription("Get Device Id")
         .WithTags("Device");
    }

    private void UpdateCookie(HttpContext ctx, string key, string? value, CookieOptions cookieOptions)
    {
        if (value != null)
        {
            ctx.Response.Cookies.Append(key, value, cookieOptions);
        }
        else
        {
            ctx.Response.Cookies.Delete(key);
        }
    }
}