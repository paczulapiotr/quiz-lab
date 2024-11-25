


namespace Quiz.Master.Extensions;
public static class HttpContextExtensions
{
    public static string GetDeviceId(this IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor?.HttpContext?.Items["DeviceId"] as string ?? "";
    }
}