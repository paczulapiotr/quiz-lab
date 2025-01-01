namespace Quiz.Common.CQRS;

public interface IBehaviorPipeline<TRequest, TResult>
{
    ValueTask<TResult> HandleAsync(TRequest command, Func<ValueTask<TResult>> next, CancellationToken cancellationToken = default);
}