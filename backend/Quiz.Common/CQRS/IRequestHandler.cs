namespace Quiz.Common.CQRS;

public interface IRequestHandler<TQuery, TResult> where TQuery : IRequest<TResult>
{
    ValueTask<TResult?> HandleAsync(TQuery request, CancellationToken cancellationToken = default);
}