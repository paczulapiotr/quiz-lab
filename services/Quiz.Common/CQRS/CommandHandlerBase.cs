
namespace Quiz.Common.CQRS;

public abstract class CommandHandlerBase<TCommand> : RequestHandlerBase<TCommand, NoResult>
    where TCommand : ICommand
{
    protected CommandHandlerBase(IEnumerable<IBehaviorPipeline<TCommand, NoResult>> behaviorPipelines) : base(behaviorPipelines)
    {
    }
}