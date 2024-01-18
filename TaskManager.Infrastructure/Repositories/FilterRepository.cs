namespace TaskManager.Infrastructure.Repositories;

public class FilterRepository : IFilterRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public FilterRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Filter Add(Filter filter)
    {
        return _context.Filters.Add(filter).Entity;
    }

    public void AddRange(IReadOnlyCollection<Filter> filters)
    {
        _context.Filters.AddRange(filters);
    }

    public void Delete(Filter filter)
    {
        _context.Filters.Remove(filter);
    }

    public async Task<Filter> GetById(Guid id)
    {
        var filter = await _context.Filters.AsNoTracking().FirstOrDefaultAsync(filter => filter.Id == id);
        return filter!;
    }

    public void Update(Filter filter)
    {
        _context.Entry(filter).State = EntityState.Modified;
    }

    public async Task<Filter> GetByName(string name)
    {
        var filter = await _context.Filters
            .Include(f => f.FilterCriterias)!
            .ThenInclude(fc => fc.Criteria)
            .AsNoTracking()
            .FirstOrDefaultAsync(filter => filter.Name == name);
        return filter!;
    }

    public async Task<FilterConfiguration?> GetConfigurationOfFilter(Guid id)
    {
        string? configuration = await _context.Filters.Where(f => f.Id == id).Select(f => f.Configuration).SingleOrDefaultAsync();
        if (string.IsNullOrWhiteSpace(configuration))
        {
            return null;
        }
        return configuration.FromJson<FilterConfiguration>();
    }
    public async Task<IReadOnlyCollection<FilterViewModel>> GetFiltersByUserId(Guid userId)
    {
        var filters = await (from f in _context.Filters.AsNoTracking().Where(f => f.CreatorUserId == userId)
                             select new FilterViewModel
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
