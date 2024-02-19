namespace TaskManager.Persistence.Repositories;

public class PriorityRepository(IDbContext context) : GenericRepository<Priority>(context)
    , IPriorityRepository
{
    public async Task<IReadOnlyCollection<Priority>> GetPrioritiesByProjectIdAsync(Guid projectId)
    {
        var priorities = await Entity
            .AsNoTracking()
            .Where(p => p.ProjectId == projectId)
            .ToListAsync();

        return priorities;
    }

    public async Task<Priority> GetMediumPriorityByProjectIdAsync(Guid projectId)
    {
        var priority = await Entity
            .AsNoTracking()
            .Where(p => p.Name == PriorityConstants.MediumName && p.ProjectId == projectId)
            .FirstOrDefaultAsync();

        return priority!;
    }

    public async Task<PaginationResult<PriorityViewModel>> GetPriorityViewModelsByProjectIdPagingAsync(Guid projectId, PaginationInput paginationInput)
    {
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        var query = from p in Entity.Where(p => p.ProjectId == projectId)
                    join i in DbContext.Set<Issue>() on p.Id equals i.PriorityId into ij
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
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'

        return await query.Pagination(paginationInput);
    }

    public async Task<string?> GetNameOfPriorityByIdAsync(Guid priorityId)
    {
        string? name = await Entity
            .AsNoTracking()
            .Where(p => p.Id == priorityId)
            .Select(p => p.Name)
            .FirstOrDefaultAsync();

        return name;
    }

    public async Task<IReadOnlyCollection<PriorityViewModel>> GetPriorityViewModelsByProjectIdAsync(Guid projectId)
    {
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        var query = from p in Entity.Where(p => p.ProjectId == projectId)
                    join i in DbContext.Set<Issue>() on p.Id equals i.PriorityId into ij
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
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'

        var priorityViewModels = await query.ToListAsync();
        return priorityViewModels.AsReadOnly();
    }
}
