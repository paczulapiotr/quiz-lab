namespace Quiz.Master.Persistance;

public class QuizSqlRepository : IQuizRepository
{
    private readonly QuizDbContext _context;

    public QuizSqlRepository(QuizDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity
    {
        _context.Remove(entity);
    }

    public async Task<TEntity?> GetAsync<TEntity>(Guid id) where TEntity : class, IEntity
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity
    {
        await _context.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public IQueryable<TEntity> Query<TEntity>(bool trackReferences = false) where TEntity : class, IEntity
    {
        return _context.Set<TEntity>().AsQueryable();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
