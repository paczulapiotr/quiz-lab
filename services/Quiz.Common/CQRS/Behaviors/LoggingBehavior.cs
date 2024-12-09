using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Quiz.Common.Extensions;

namespace Quiz.Common.CQRS.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger, IHttpContextAccessor httpContextAccessor) : IBehaviorPipeline<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    public async ValueTask<TResponse> HandleAsync(TRequest request, Func<ValueTask<TResponse>> next, CancellationToken cancellationToken = default)
    {
        var correlationId = httpContextAccessor.GetCorrelationId();
        logger.LogTrace("[{CorrelationId}/START] Handling Request={Request} - Response={Response} - RequestData={RequestData}",
                    correlationId, typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = Stopwatch.StartNew();
        var response = await next();
        timer.Stop();
        var timeTaken = timer.Elapsed;
        if (timeTaken.Seconds > 3)
        {
            logger.LogWarning("[{CorrelationId}/SLOW] Request={Request} - Response={Response} - TimeTaken={TimeTaken}",
                correlationId, typeof(TRequest).Name, typeof(TResponse).Name, timeTaken);
        }

        return response;
    }
}