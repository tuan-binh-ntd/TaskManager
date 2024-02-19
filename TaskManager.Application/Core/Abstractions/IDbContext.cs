namespace TaskManager.Application.Core.Abstractions;

public interface IDbContext
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : BaseEntity;

    Task<TEntity?> GetBydIdAsync<TEntity>(Guid id)
        where TEntity : BaseEntity;

    void Insert<TEntity>(TEntity entity)
        where TEntity : BaseEntity;

    void InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities)
        where TEntity : BaseEntity;

    void Remove<TEntity>(TEntity entity)
        where TEntity : BaseEntity;

    void RemoveRange<TEntity>(IReadOnlyCollection<TEntity> entities)
        where TEntity : BaseEntity;

    Task<int> ExecuteSqlAsync(string sql, IEnumerable<SqlParameter> parameters, CancellationToken cancellationToken = default);

    DbSet<AppUser> AppUser { get; }
}
