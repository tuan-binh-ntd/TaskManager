namespace TaskManager.Persistence.Repositories;

public class BacklogRepository(IDbContext context) : GenericRepository<Backlog>(context)
    , IBacklogRepository
{

    public async Task<IReadOnlyCollection<Issue>> GetIssuesByBacklogIdAsync(Guid backlogId)
    {
        var projectId = await Entity
            .AsNoTracking()
            .Where(b => b.Id == backlogId)
            .Select(b => b.ProjectId)
            .FirstOrDefaultAsync();

        var subtaskTypeId = await DbContext.Set<IssueType>()
            .AsNoTracking()
            .Where(it => it.ProjectId == projectId && it.Name == IssueTypeConstants.SubtaskName)
            .Select(it => it.Id)
            .FirstOrDefaultAsync();

        var issues = await DbContext.Set<Issue>()
            .Where(i => i.BacklogId == backlogId && i.IssueTypeId != subtaskTypeId)
            .ToListAsync();
        return issues.AsReadOnly();
    }

    public async Task<BacklogViewModel?> GetBacklogByProjectIdAsync(Guid projectId)
    {
        var backlog = await Entity
            .Where(b => b.ProjectId == projectId)
            .Select(b => new BacklogViewModel
            {
                Id = b.Id,
                Name = b.Name,
            })
            .FirstOrDefaultAsync();

        return backlog;
    }

    public async Task<IReadOnlyCollection<Guid>> GetBacklogIdsByProjectIdsAsync(IReadOnlyCollection<Guid> projectIds)
    {
        var backlogIds = await Entity.AsNoTracking()
            .Where(s => projectIds.Contains(s.ProjectId))
            .Select(s => s.Id)
            .ToListAsync();

        return backlogIds.AsReadOnly();
    }
}
