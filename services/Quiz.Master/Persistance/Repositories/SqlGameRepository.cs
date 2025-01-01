
using Microsoft.EntityFrameworkCore;
using Quiz.Master.Persistance.Repositories.Abstract;

namespace Quiz.Master.Persistance.Repositories;

public class SqlGameRepository : IGameRepository
{
    private readonly QuizDbContext dbContext;

    public SqlGameRepository(IDbContextFactory<QuizDbContext> dbContextFactory)
    {
        dbContext = dbContextFactory.CreateDbContext();
    }

    public async Task<Core.Models.Game> FindAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Games
            .Include(g => g.Players).ThenInclude(x => x.Scores)
            .Include(x => x.MiniGames)
            .ThenInclude(x => x.MiniGameDefinition)
            .FirstAsync(g => g.Id == gameId, cancellationToken);
    }



    public async Task UpdateAsync(Core.Models.Game game, CancellationToken cancellationToken = default)
    {
        var gameEntity = await dbContext.Games.FirstOrDefaultAsync(x => x.Id == game.Id, cancellationToken);

        if (gameEntity is null)
        {
            throw new InvalidOperationException("Game not found");
        }
        else
        {
            dbContext.Entry(gameEntity).CurrentValues.SetValues(game);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

    }
}
