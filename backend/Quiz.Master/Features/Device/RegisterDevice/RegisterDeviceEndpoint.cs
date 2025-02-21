using Carter;
using Quiz.Common;
using Quiz.Master.Core.Models;
using Quiz.Master.Persistance.Repositories.Abstract;

namespace Quiz.Master.Features.Device.RegisterDevice;

public record RegisterDeviceRequestDto(string? UniqueId, string RoomCode, bool IsHost = false);
public record RegisterDeviceResponseDto(string? RoomCode, string? UniqueId, bool Ok);

public class RegisterDeviceEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/device/register", async (RegisterDeviceRequestDto dto, IGameRepository repository, IHttpContextAccessor httpContextAccessor, CancellationToken token = default) =>
         {
             var uniqueId = dto.UniqueId ?? IdGenerator.New;
             Room? room = null;
             if (dto.IsHost)
             {
                 room = await repository.RegisterHostAsync(uniqueId, dto.RoomCode, token);
             }
             else
             {
                 room = await repository.RegisterPlayerAsync(uniqueId, dto.RoomCode, token);
             }

             var cookieOptions = new CookieOptions
             {
                 HttpOnly = true,
                 Secure = true,
                 SameSite = SameSiteMode.None,
             };

             if (room != null)
             {
                 if (dto.IsHost)
                 {

                     httpContextAccessor?.HttpContext?.Response.Cookies.Append("hostId", uniqueId, cookieOptions);
                     httpContextAccessor?.HttpContext?.Response.Cookies.Delete("deviceId");
                 }
                 else
                 {
                     httpContextAccessor?.HttpContext?.Response.Cookies.Append("deviceId", uniqueId, cookieOptions);
                     httpContextAccessor?.HttpContext?.Response.Cookies.Delete("hostId");
                 }

                 httpContextAccessor?.HttpContext?.Response.Cookies.Append("roomCode", dto.RoomCode, cookieOptions);
             }
             else
             {
                 httpContextAccessor?.HttpContext?.Response.Cookies.Delete("hostId");
                 httpContextAccessor?.HttpContext?.Response.Cookies.Delete("deviceId");
                 httpContextAccessor?.HttpContext?.Response.Cookies.Delete("roomCode");
             }

             return Results.Ok(room != null
                ? new RegisterDeviceResponseDto(dto.RoomCode, uniqueId, true)
                : new RegisterDeviceResponseDto(null, null, false));
         })
         .WithName("Register Device")
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithSummary("Register Device")
         .WithDescription("Register Device")
         .WithTags("Device");
    }
}