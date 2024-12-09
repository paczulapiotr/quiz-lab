
using Microsoft.EntityFrameworkCore;
using Quiz.Master.Persistance;

namespace Quiz.Master.Game.Repository;

public class GameStateSqlRepository : IGameStateRepository
{
    private readonly QuizDbContext dbContext;

    public GameStateSqlRepository(IDbContextFactory<QuizDbContext> dbContextFactory)
    {
        dbContext = dbContextFactory.CreateDbContext();
    }

    public async Task<Persistance.Models.Game> GetGame(Guid gameId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Games
            .Include(g => g.Players)
            .Include(x => x.MiniGames)
            .FirstAsync(g => g.Id == gameId, cancellationToken);
    }

    public async Task SaveGameState(Persistance.Models.Game game, CancellationToken cancellationToken = default)
    {
        dbContext.Update(game);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
