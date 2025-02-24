


namespace Quiz.Master.Extensions;
public static class HttpContextExtensions
{
    public static string? GetUniqueId(this IHttpContextAccessor httpContextAccessor)
    {
        var ctx = httpContextAccessor.HttpContext;
        return ctx?.Request.Headers["deviceId"] ?? ctx?.Request.Headers["hostId"];
    }

    public static string? GetRoomCode(this IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor.HttpContext?.Request.Headers["roomCode"];
    }
}