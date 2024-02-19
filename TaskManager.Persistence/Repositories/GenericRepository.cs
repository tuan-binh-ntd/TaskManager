namespace TaskManager.Persistence.Repositories;

public abstract class GenericRepository<TEntity>
    where TEntity : BaseEntity
{
    protected GenericRepository(IDbContext dbContext) => DbContext = dbContext;

    protected IDbContext DbContext { get; }

    protected DbSet<TEntity> Entity => DbContext.Set<TEntity>();

    public async Task<TEntity?> GetByIdAsync(Guid id) => await DbContext.GetBydIdAsync<TEntity>(id);

    public void Insert(TEntity entity) => DbContext.Insert(entity);

    public void InsertRange(IReadOnlyCollection<TEntity> entities) => DbContext.InsertRange(entities);

    public void Update(TEntity entity) => DbContext.Set<TEntity>().Update(entity);

    public void UpdateRange(IReadOnlyCollection<TEntity> entities) => DbContext.Set<TEntity>().UpdateRange(entities);

    public void Remove(TEntity entity) => DbContext.Remove(entity);

    public void RemoveRange(IReadOnlyCollection<TEntity> entities) => DbContext.RemoveRange(entities);
}
