using Carter;
using Quiz.Common;
using Quiz.Master.Core.Models;
using Quiz.Master.Persistance.Repositories.Abstract;

namespace Quiz.Master.Features.Device.RegisterDevice;

public record RegisterDeviceRequestDto(string? UniqueId, string? RoomCode, bool IsHost = false);
public record RegisterDeviceResponseDto(string? RoomCode, string? UniqueId, bool Ok);

public class RegisterDeviceEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/device/register", async (RegisterDeviceRequestDto dto, IGameRepository repository, IHttpContextAccessor httpContextAccessor, ILogger<RegisterDeviceEndpoint> logger, CancellationToken token = default) =>
         {
             var uniqueId = dto.UniqueId ?? IdGenerator.New;
             Room? room = null;
             string? errorCode = null;
             if (dto.IsHost)
             {
                 var roomCode = string.IsNullOrWhiteSpace(dto.RoomCode) ? IdGenerator.New : dto.RoomCode;
                 (room, errorCode) = await repository.RegisterHostAsync(uniqueId, roomCode, token);
             }
             else if (string.IsNullOrWhiteSpace(dto.RoomCode))
             {
                 logger.LogError("Error registering device {uniqueId} for roomCode {roomCode}: {errorCode}", uniqueId, dto.RoomCode, "RoomCode is required");
                 return new RegisterDeviceResponseDto(null, null, false);
             }
             else
             {
                 (room, errorCode) = await repository.RegisterPlayerAsync(uniqueId, dto.RoomCode, token);
             }

             if (!string.IsNullOrWhiteSpace(errorCode))
             {
                 logger.LogError("Error registering device {uniqueId} for roomCode {roomCode}: {errorCode}", uniqueId, dto.RoomCode, errorCode);
                 return new RegisterDeviceResponseDto(null, null, false);
             }

             return room != null
                ? new RegisterDeviceResponseDto(room.Code, uniqueId, true)
                : new RegisterDeviceResponseDto(null, null, false);
         })
         .WithName("Register Device")
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithSummary("Register Device")
         .WithDescription("Register Device")
         .WithTags("Device");
    }
}