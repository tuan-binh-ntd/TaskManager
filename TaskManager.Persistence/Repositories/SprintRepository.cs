namespace TaskManager.Persistence.Repositories;

public class SprintRepository(IDbContext context) : GenericRepository<Sprint>(context)
    , ISprintRepository
{
    public async Task<IReadOnlyCollection<Issue>> GetIssuesBySprintIdAsync(Guid sprintId, Guid projectId)
    {
        var subtaskTypeId = await DbContext.Set<IssueType>()
            .AsNoTracking()
            .Where(it => it.ProjectId == projectId && it.Name == IssueTypeConstants.SubtaskName)
            .Select(it => it.Id)
            .FirstOrDefaultAsync();

        var issues = await DbContext.Set<Issue>()
            .Where(i => i.SprintId == sprintId && i.IssueTypeId != subtaskTypeId)
            .ToListAsync();

        return issues.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<SprintViewModel>> GetSprintsByProjectIdAsync(Guid projectId)
    {
        var sprints = await Entity
            .AsNoTracking()
            .Where(e => e.ProjectId == projectId && e.IsComplete != true)
            .ProjectToType<SprintViewModel>()
            .ToListAsync();

        return sprints.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Guid>> GetSprintIdsByProjectIdAsync(Guid projectId)
    {
        var sprints = await Entity
            .AsNoTracking()
            .Where(e => e.ProjectId == projectId && e.IsComplete != true && e.IsStart == true)
            .Select(s => s.Id)
            .ToListAsync();

        return sprints.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Issue>> GetIssuesBySprintIdAsync(Guid sprintId)
    {
        var issues = await DbContext.Set<Issue>()
            .Where(i => i.SprintId == sprintId)
            .ToListAsync();

        return issues.AsReadOnly();
    }

    public async Task<string?> GetNameOfSprintAsync(Guid sprintId)
    {
        string? name = await Entity
            .AsNoTracking()
            .Where(s => s.Id == sprintId)
            .Select(s => s.Name)
            .FirstOrDefaultAsync();

        return name;
    }

    public async Task<IReadOnlyCollection<Guid>> GetSprintIdsByProjectIdsAsync(IReadOnlyCollection<Guid> projectIds)
    {
        var sprintIds = await Entity
            .AsNoTracking()
            .Where(s => projectIds.Contains(s.ProjectId))
            .Select(s => s.Id)
            .ToListAsync();

        return sprintIds.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<SprintViewModel>> GetSprintViewModelsByProjectIdAsync(Guid projectId)
    {
        var sprints = await Entity
            .AsNoTracking()
            .Where(e => e.ProjectId == projectId)
            .ProjectToType<SprintViewModel>()
            .ToListAsync();

        return sprints.AsReadOnly();
    }
}
