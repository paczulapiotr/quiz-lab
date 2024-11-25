
using Quiz.Common.CQRS;
using Quiz.Master.Persistance;

namespace Quiz.Master.Features.Game.JoinGame;

public record JoinGameCommand(string DeviceId, string PlayerName) : ICommand;

public class JoinGameHandler : ICommandHandler<JoinGameCommand>
{
    private readonly IQuizRepository quizRepository;

    public JoinGameHandler(IQuizRepository quizRepository)
    {
        this.quizRepository = quizRepository;
    }
    public async ValueTask<NoResult?> HandleAsync(JoinGameCommand request, CancellationToken cancellationToken = default)
    {
        return await ValueTask.FromResult(NoResult.Instance);
    }
}
