namespace Quiz.Common.CQRS;

public interface IQuery<TResult> : IRequest<TResult>
{
}