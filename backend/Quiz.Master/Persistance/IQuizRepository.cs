using Quiz.Master.Core;

namespace Quiz.Master.Persistance;

public interface IQuizRepository
{
    public Task AddAsync<TEntity>(TEntity entity) where TEntity : class, IEntity;

    public Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity;

    public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity;

    public Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity;
    public Task<TEntity?> GetAsync<TEntity>(Guid id) where TEntity : class, IEntity;
    public IQueryable<TEntity> Query<TEntity>(bool trackReferences = false) where TEntity : class, IEntity;

    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
