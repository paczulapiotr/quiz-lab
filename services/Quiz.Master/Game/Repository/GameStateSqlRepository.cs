
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
            .Include(g => g.Players).ThenInclude(x => x.Scores)
            .Include(x => x.MiniGames)
            .ThenInclude(x => x.MiniGameDefinition)
            .FirstAsync(g => g.Id == gameId, cancellationToken);
    }


    public async Task Save<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, IEntity
    {
        var existingEntity = await dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken);

        if (existingEntity is null)
        {
            dbContext.Add(entity);
        }
        else
        {
            dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
