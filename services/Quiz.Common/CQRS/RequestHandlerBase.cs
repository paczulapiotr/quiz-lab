namespace Quiz.Common.CQRS;
public abstract class RequestHandlerBase<TRequest, TResult> : IRequestHandler<TRequest, TResult> where TRequest : IRequest<TResult>
{
    private readonly IEnumerable<IBehaviorPipeline<TRequest, TResult>> _behaviorPipelines;

    protected RequestHandlerBase(IEnumerable<IBehaviorPipeline<TRequest, TResult>> behaviorPipelines)
    {
        _behaviorPipelines = behaviorPipelines;
    }

    public async ValueTask<TResult?> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        TResult? result = default;
        var pipeline = _behaviorPipelines.Reverse().Aggregate(
            () => ProcessAsync(request, cancellationToken),
            (next, pipeline) => () => pipeline.HandleAsync(request, next, cancellationToken));

        await pipeline();

        return result;
    }

    protected abstract ValueTask<TResult> ProcessAsync(TRequest request, CancellationToken cancellationToken);
}