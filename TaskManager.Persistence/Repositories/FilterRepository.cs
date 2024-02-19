namespace TaskManager.Persistence.Repositories;

public class FilterRepository(IDbContext context) : GenericRepository<Filter>(context)
    , IFilterRepository
{
    public async Task<Filter> GetByNameAsync(string name)
    {
        var filter = await Entity
            .Include(f => f.FilterCriterias)!
            .ThenInclude(fc => fc.Criteria)
            .AsNoTracking()
            .FirstOrDefaultAsync(filter => filter.Name == name);
        return filter!;
    }

    public async Task<FilterConfiguration?> GetConfigurationOfFilterAsync(Guid id)
    {
        string? configuration = await Entity
            .Where(f => f.Id == id)
            .Select(f => f.Configuration)
            .SingleOrDefaultAsync();

        if (string.IsNullOrWhiteSpace(configuration))
        {
            return null;
        }

        return configuration.FromJson<FilterConfiguration>();
    }
    public async Task<IReadOnlyCollection<FilterViewModel>> GetFiltersByUserIdAsync(Guid userId)
    {
        var filters = await Entity
            .AsNoTracking()
            .Where(f => f.CreatorUserId == userId)
            .Select(f => new FilterViewModel
            {
                Id = f.Id,
                Name = f.Name,
                Type = f.Type,
                Stared = f.Stared,
                Configuration = string.IsNullOrWhiteSpace(f.Configuration) ? new FilterConfiguration() : f.Configuration.FromJson<FilterConfiguration>()
            })
            .OrderBy(f => f.Type)
            .ToListAsync();

        return filters.AsReadOnly();
    }
}
