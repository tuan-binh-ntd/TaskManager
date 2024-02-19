namespace TaskManager.Persistence.Repositories;

public class ProjectConfigurationRepository(IDbContext context) : GenericRepository<ProjectConfiguration>(context)
    , IProjectConfigurationRepository
{
    public async Task<ProjectConfiguration?> GetProjectConfigurationByProjectIdAsync(Guid projectId)
    {
        return await Entity
            .AsNoTracking()
            .Where(e => e.ProjectId == projectId)
            .FirstOrDefaultAsync();
    }

    public async Task<Guid?> GetDefaultAssigneeIdByProjectIdAsync(Guid projectId)
    {
        var assigneeId = await Entity
            .Where(pc => pc.ProjectId == projectId)
            .Select(pc => pc.DefaultAssigneeId)
            .FirstOrDefaultAsync();

        return assigneeId;
    }

    public async Task UpdateDefaultAssigneeAsync(Guid projectId, Guid? defaultAssigneeId)
    {
        await Entity
            .Where(pc => pc.ProjectId == projectId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.DefaultAssigneeId, defaultAssigneeId));
    }
}
