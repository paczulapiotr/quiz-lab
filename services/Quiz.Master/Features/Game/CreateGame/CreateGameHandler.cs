
using Quiz.Common.Broker.Publisher;
using Quiz.Common.CQRS;
using Quiz.Common.Messages.Game;
using Quiz.Master.Persistance;
using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Features.Game.CreateGame;

public record CreateGameCommand(uint GameSize) : ICommand;

public class CreateGameHandler : ICommandHandler<CreateGameCommand>
{
    private readonly IQuizRepository quizRepository;
    private readonly IPublisher publisher;

    public CreateGameHandler(IQuizRepository quizRepository, IPublisher publisher)
    {
        this.quizRepository = quizRepository;
        this.publisher = publisher;
    }
    public async ValueTask<NoResult?> HandleAsync(CreateGameCommand request, CancellationToken cancellationToken = default)
    {
        var game = new Persistance.Models.Game
        {
            GameSize = request.GameSize,
            MiniGames = []
        };
        await quizRepository.AddAsync(game);

        await quizRepository.SaveChangesAsync();

        await publisher.PublishAsync(new GameStatusUpdate(game.Id.ToString(), GameStatus.GameCreated), cancellationToken);

        return NoResult.Instance;
    }
}
