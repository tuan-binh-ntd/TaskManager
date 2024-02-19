namespace TaskManager.Persistence.Repositories;

public class IssueTypeRepository(IDbContext context) : GenericRepository<IssueType>(context)
    , IIssueTypeRepository
{
    public async Task<IReadOnlyCollection<IssueTypeViewModel>> GetIssueTypesByProjectIdAsync(Guid projectId)
    {
        var issueTypes = await Entity
            .AsNoTracking()
            .Where(e => e.ProjectId == projectId)
            .Select(e => new IssueTypeViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Icon = e.Icon,
                Level = e.Level,
            })
            .ToListAsync();

        return issueTypes.AsReadOnly();
    }

    public async Task<IssueType?> GetSubtaskAsync()
    {
        var issueType = await Entity
            .AsNoTracking()
            .Where(e => e.Name == IssueTypeConstants.SubtaskName)
            .FirstOrDefaultAsync();

        return issueType;
    }

    public async Task<IssueType?> GetEpicAsync(Guid projectId)
    {
        var issueType = await Entity
            .AsNoTracking()
            .Where(e => e.Name == IssueTypeConstants.EpicName && e.ProjectId == projectId)
            .FirstOrDefaultAsync();

        return issueType;
    }

    public async Task<PaginationResult<IssueTypeViewModel>> GetIssueTypeViewModelsByProjectIdPagingAsync(Guid projectId, PaginationInput paginationInput)
    {
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        var query = from it in Entity.AsNoTracking().Where(it => it.ProjectId == projectId)
                    join i in DbContext.Set<Issue>().AsNoTracking() on it.Id equals i.IssueTypeId into ij
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
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'

        return await query.Pagination(paginationInput);
    }

    public async Task<string?> GetNameOfIssueTypeAsync(Guid issueTypeId)
    {
        string? name = await Entity
            .AsNoTracking()
            .Where(it => it.Id == issueTypeId)
            .Select(it => it.Name)
            .FirstOrDefaultAsync();

        return name;
    }

    public async Task<IReadOnlyCollection<IssueTypeViewModel>> GetIssueTypeViewModelsByProjectIdAsync(Guid projectId)
    {
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        var issueTypes = await (from it in Entity.AsNoTracking().Where(it => it.ProjectId == projectId)
                                join i in DbContext.Set<Issue>().AsNoTracking() on it.Id equals i.IssueTypeId into ij
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
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'

        return issueTypes.AsReadOnly();
    }
}
