namespace TaskManager.Persistence.Repositories;

public class VersionRepository(IDbContext context) : GenericRepository<Version>(context)
    , IVersionRepository
{
    public async Task<IReadOnlyCollection<Version>> GetVersionsByProjectIdAsync(Guid projectId)
    {
        var versions = await Entity
            .AsNoTracking()
            .Where(e => e.ProjectId == projectId)
            .ToListAsync();

        return versions.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<VersionViewModel>> GetStatusViewModelsByIssueIdAsync(Guid issueId)
    {
        var versionViewModels = await (from vi in DbContext.Set<VersionIssue>().Where(vi => vi.IssueId == issueId)
                                       join v in Entity on vi.VersionId equals v.Id
                                       select new VersionViewModel
                                       {
                                           Id = v.Id,
                                           Name = v.Name,
                                       })
                                       .ToListAsync();

        return versionViewModels.AsReadOnly();
    }

    public async Task<int> IssuesNotDoneNumInVersionAsync(Guid versionId)
    {
        var query = from vi in DbContext.Set<VersionIssue>().Where(vi => vi.VersionId == versionId)
                    join i in DbContext.Set<Issue>() on vi.IssueId equals i.Id
                    join s in DbContext.Set<Status>() on i.StatusId equals s.Id
                    join sc in DbContext.Set<StatusCategory>() on s.StatusCategoryId equals sc.Id
                    where sc.Code == StatusCategoryConstants.ToDoCode || sc.Code == StatusCategoryConstants.InProgressCode
                    select i.Id;

        return await query.CountAsync();
    }

    public async Task<IReadOnlyCollection<Guid>> IssuesNotDoneInVersionAsync(Guid versionId)
    {
        var issueIds = await (from vi in DbContext.Set<VersionIssue>().Where(vi => vi.VersionId == versionId)
                              join i in DbContext.Set<Issue>() on vi.IssueId equals i.Id
                              join s in DbContext.Set<Status>() on i.StatusId equals s.Id
                              join sc in DbContext.Set<StatusCategory>() on s.StatusCategoryId equals sc.Id
                              where sc.Code == StatusCategoryConstants.ToDoCode || sc.Code == StatusCategoryConstants.InProgressCode
                              select i.Id).ToListAsync();

        return issueIds.AsReadOnly();
    }
}
