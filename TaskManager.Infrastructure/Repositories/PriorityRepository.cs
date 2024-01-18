namespace TaskManager.Infrastructure.Repositories;

public class PriorityRepository : IPriorityRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public PriorityRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Priority Add(Priority priority)
    {
        return _context.Priorities.Add(priority).Entity;
    }

    public void AddRange(IReadOnlyCollection<Priority> priorities)
    {
        _context.Priorities.AddRange(priorities);
    }

    public void Delete(Guid id)
    {
        var priority = _context.Priorities.FirstOrDefault(p => p.Id == id);
        _context.Priorities.Remove(priority!);
    }

    public async Task<Priority> GetById(Guid id)
    {
        var priority = await _context.Priorities.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return priority!;
    }

    public async Task<IReadOnlyCollection<Priority>> GetByProjectId(Guid projectId)
    {
        var priorities = await _context.Priorities.AsNoTracking().Where(p => p.ProjectId == projectId).ToListAsync();
        return priorities!;
    }

    public void Update(Priority priority)
    {
        _context.Entry(priority).State = EntityState.Modified;
    }

    public async Task<Priority> GetMediumByProjectId(Guid projectId)
    {
        var priority = await _context.Priorities.AsNoTracking().FirstOrDefaultAsync(p => p.Name == CoreConstants.MediumName && p.ProjectId == projectId);
        return priority!;
    }

    public async Task<PaginationResult<PriorityViewModel>> GetByProjectId(Guid projectId, PaginationInput paginationInput)
    {
        var query = from p in _context.Priorities.Where(p => p.ProjectId == projectId)
                    join i in _context.Issues on p.Id equals i.PriorityId into ij
                    from ilj in ij.DefaultIfEmpty()
                    group new { p, ilj } by new { p.Id, p.Name, p.Description, p.IsMain, p.Color, p.Icon, p.ProjectId } into g
                    select new PriorityViewModel
                    {
                        Id = g.Key.Id,
                        Name = g.Key.Name,
                        Description = g.Key.Description,
                        IsMain = g.Key.IsMain,
                        Color = g.Key.Color,
                        Icon = g.Key.Icon,
                        ProjectId = (Guid)g.Key.ProjectId!,
                        IssueCount = g.Count(g => g.ilj.Id != null),
                    };

        return await query.Pagination(paginationInput);
    }

    public async Task<string?> GetNameOfPriority(Guid priorityId)
    {
        string? name = await _context.Priorities.AsNoTracking().Where(p => p.Id == priorityId).Select(p => p.Name).FirstOrDefaultAsync();
        return name;
    }

    public async Task<IReadOnlyCollection<PriorityViewModel>> GetPriorityViewModelsByProjectId(Guid projectId)
    {
        var query = from p in _context.Priorities.Where(p => p.ProjectId == projectId)
                    join i in _context.Issues on p.Id equals i.PriorityId into ij
                    from ilj in ij.DefaultIfEmpty()
                    group new { p, ilj } by new { p.Id, p.Name, p.Description, p.IsMain, p.Color, p.Icon, p.ProjectId } into g
                    select new PriorityViewModel
                    {
                        Id = g.Key.Id,
                        Name = g.Key.Name,
                        Description = g.Key.Description,
                        IsMain = g.Key.IsMain,
                        Color = g.Key.Color,
                        Icon = g.Key.Icon,
                        ProjectId = (Guid)g.Key.ProjectId!,
                        IssueCount = g.Count(g => g.ilj.Id != null),
                    };

        var priorityViewModels = await query.ToListAsync();
        return priorityViewModels.AsReadOnly();
    }
}
