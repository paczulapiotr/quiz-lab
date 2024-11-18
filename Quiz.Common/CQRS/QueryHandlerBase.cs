
namespace Quiz.Common.CQRS;

public abstract class QueryHandlerBase<TQuery, TResult> : RequestHandlerBase<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    protected QueryHandlerBase(IEnumerable<IBehaviorPipeline<TQuery, TResult>> behaviorPipelines) : base(behaviorPipelines)
    {
    }

}