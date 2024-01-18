using TaskManager.Core.Helper;

namespace TaskManager.Infrastructure.Repositories;

public class IssueTypeRepository : IIssueTypeRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public IssueTypeRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IssueTypeViewModel Add(IssueType issueType)
    {
        return _context.IssueTypes.Add(issueType).Entity.Adapt<IssueTypeViewModel>();
    }

    public void Delete(Guid id)
    {
        var issueType = _context.IssueTypes.SingleOrDefault(e => e.Id == id);
        _context.IssueTypes.Remove(issueType!);
    }

    public async Task<IReadOnlyCollection<IssueTypeViewModel>> Gets()
    {
        var issueTypes = await _context.IssueTypes.AsNoTracking().Select(e => new IssueTypeViewModel()
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            Icon = e.Icon,
            Level = e.Level,
        }).ToListAsync();
        return issueTypes.AsReadOnly();
    }

    public void Update(IssueType issueType)
    {
        _context.Entry(issueType).State = EntityState.Modified;
    }

    public async Task<IReadOnlyCollection<IssueTypeViewModel>> GetsByProjectId(Guid projectId)
    {
        var issueTypes = await _context.IssueTypes.AsNoTracking()
            .Where(e => e.ProjectId == projectId).Select(e => new IssueTypeViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Icon = e.Icon,
                Level = e.Level,
            }).ToListAsync();
        return issueTypes.AsReadOnly();
    }

    public async Task<IssueType> Get(Guid id)
    {
        var issueType = await _context.IssueTypes.AsNoTracking().Where(e => e.Id == id).SingleOrDefaultAsync();
        return issueType!;
    }

    public async Task<IssueType> GetSubtask()
    {
        var issueType = await _context.IssueTypes.AsNoTracking().
           Where(e => e.Name == CoreConstants.SubTaskName).SingleOrDefaultAsync();
        return issueType!;
    }

    public async Task<IssueType> GetEpic(Guid projectId)
    {
        var issueType = await _context.IssueTypes.AsNoTracking()
            .Where(e => e.Name == CoreConstants.EpicName && e.ProjectId == projectId).SingleOrDefaultAsync();
        return issueType!;
    }

    public async Task<PaginationResult<IssueTypeViewModel>> GetsByProjectIdPaging(Guid projectId, PaginationInput paginationInput)
    {
        var query = from it in _context.IssueTypes.AsNoTracking().Where(it => it.ProjectId == projectId)
                    join i in _context.Issues.AsNoTracking() on it.Id equals i.IssueTypeId into ij
                    from ilj in ij.DefaultIfEmpty()
                    group new { it, ilj } by new { it.Id, it.Name, it.Description, it.Icon, it.Level, it.IsMain } into g
                    select new IssueTypeViewModel
                    {
                        Id = g.Key.Id,
                        Name = g.Key.Name,
                        Description = g.Key.Description,
                        Icon = g.Key.Icon,
                        Level = g.Key.Level,
                        IsMain = g.Key.IsMain,
                        IssueCount = g.Count(g => g.ilj.Id != null)
                    };

        return await query.Pagination(paginationInput);
    }

    public void AddRange(IReadOnlyCollection<IssueType> issueTypes)
    {
        _context.IssueTypes.AddRange(issueTypes);
    }

    public async Task<string?> GetNameOfIssueType(Guid issueTypeId)
    {
        string? name = await _context.IssueTypes.AsNoTracking().Where(it => it.Id == issueTypeId).Select(it => it.Name).FirstOrDefaultAsync();
        return name;
    }

    public async Task<IReadOnlyCollection<IssueTypeViewModel>> GetIssueTypeViewModelsByProjectId(Guid projectId)
    {
        var issueTypes = await (from it in _context.IssueTypes.AsNoTracking().Where(it => it.ProjectId == projectId)
                                join i in _context.Issues.AsNoTracking() on it.Id equals i.IssueTypeId into ij
                                from ilj in ij.DefaultIfEmpty()
                                group new { it, ilj } by new { it.Id, it.Name, it.Description, it.Icon, it.Level, it.IsMain } into g
                                select new IssueTypeViewModel
                                {
                                    Id = g.Key.Id,
                                    Name = g.Key.Name,
                                    Description = g.Key.Description,
                                    Icon = g.Key.Icon,
                                    Level = g.Key.Level,
                                    IsMain = g.Key.IsMain,
                                    IssueCount = g.Count(g => g.ilj.Id != null)
                                }).ToListAsync();

        return issueTypes.AsReadOnly();
    }
}
