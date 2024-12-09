


namespace Quiz.Master.Extensions;
public static class HttpContextExtensions
{
    public static string GetDeviceId(this IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor.HttpContext?.Request?.Headers["DeviceId"].FirstOrDefault() ?? "";
    }
}