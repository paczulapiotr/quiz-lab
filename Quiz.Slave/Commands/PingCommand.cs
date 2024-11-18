using Quiz.Common.Broker.Publisher;
using Quiz.Common.CQRS;
using Quiz.Common.Extensions;
using Quiz.Common.Messages.PingPong;

namespace Quiz.Slave.Commands;

public record PingCommand(string Message) : ICommand
{
}

public interface IPingCommandHandler : ICommandHandler<PingCommand>
{
}

public class PingCommandHandler
 : CommandHandlerBase<PingCommand>, IPingCommandHandler
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IPublisher publisher;

    public PingCommandHandler(
        IEnumerable<IBehaviorPipeline<PingCommand, NoResult>> behaviorPipelines,
        IHttpContextAccessor httpContextAccessor,
        IPublisher publisher) : base(behaviorPipelines)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.publisher = publisher;
    }

    protected override async ValueTask<NoResult> ProcessAsync(PingCommand request, CancellationToken cancellationToken)
    {
        await publisher.PublishAsync(new Ping(request.Message, httpContextAccessor.GetCorrelationId()), cancellationToken);

        return NoResult.Instance;
    }
}