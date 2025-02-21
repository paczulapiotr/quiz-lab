


namespace Quiz.Master.Extensions;
public static class HttpContextExtensions
{
    public static string? GetUniqueId(this IHttpContextAccessor httpContextAccessor)
    {
        var ctx = httpContextAccessor.HttpContext;
        return ctx?.Request.Cookies["deviceId"] ?? ctx?.Request.Cookies["hostId"];
    }

    public static string? GetRoomCode(this IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor.HttpContext?.Request.Cookies["roomCode"];
    }
}