namespace TaskManager.Persistence.Repositories;

public class CriteriaRepository(IDbContext context) : GenericRepository<Criteria>(context)
    , ICriteriaRepository
{
    public async Task<IReadOnlyCollection<Criteria>> GetCriteriasAsync()
    {
        var criterias = await Entity
            .AsNoTracking()
            .ToListAsync();

        return criterias.AsReadOnly();
    }
}
