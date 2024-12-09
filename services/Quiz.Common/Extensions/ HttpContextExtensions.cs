using Microsoft.AspNetCore.Http;

namespace Quiz.Common.Extensions;

public static class HttpContextExtensions
{
    public static string? GetCorrelationId(this IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor?.HttpContext?.Items["CorrelationId"] as string;
    }
}