using Microsoft.AspNetCore.Http;

namespace Quiz.Common.Middlewares;

public class CorrelationIdMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var newCorrelationId = IdGenerator.New;
        context.Items["CorrelationId"] = newCorrelationId;
        context.Response.Headers.Add("X-Correlation-ID", newCorrelationId);

        await next(context);
    }
}