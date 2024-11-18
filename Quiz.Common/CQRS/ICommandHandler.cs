namespace Quiz.Common.CQRS;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, NoResult> where TCommand : ICommand
{
}