namespace TaskManager.Persistence.Repositories;

public class IssueRepository(IDbContext context) : GenericRepository<Issue>(context)
    , IIssueRepository
{
    public async Task<IReadOnlyCollection<Issue>> GetIssuesBySprintIdAsync(Guid sprintId)
    {
        var issues = await Entity
            .Where(i => i.SprintId == sprintId)
            .ToListAsync();

        return issues.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Issue>> GetIssuesByBacklogIdAsync(Guid backlogId)
    {
        var issues = await Entity
            .Where(i => i.BacklogId == backlogId)
            .ToListAsync();

        return issues.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Issue>> GetChildIssuesByParentIdAsync(Guid parentId)
    {
        var childIssues = await Entity
            .Where(i => i.ParentId == parentId)
            .ToListAsync();

        return childIssues.AsReadOnly();
    }

    public async Task LoadEntitiesRelationshipAsync(Issue issue)
    {
        await Entity.Entry(issue).Reference(i => i.IssueType).LoadAsync();
        await Entity.Entry(issue).Reference(i => i.IssueDetail).LoadAsync();
        await Entity.Entry(issue).Collection(i => i.Attachments!).LoadAsync();
        await Entity.Entry(issue).Collection(i => i.IssueHistories!).LoadAsync();
        await Entity.Entry(issue).Collection(i => i.Comments!).LoadAsync();
        await Entity.Entry(issue).Reference(i => i.Status).LoadAsync();
    }

    public async Task<IReadOnlyCollection<Issue>> GetIssuesByIdsAsync(IReadOnlyCollection<Guid> ids)
    {
        var issues = await Entity
            .Where(e => ids.Contains(e.Id))
            .ToListAsync();

        return issues.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Issue>> GetEpicsByProjectIdAsync(Guid projectId)
    {
        var epics = await Entity
            .Where(e => e.SprintId == null && e.BacklogId == null && e.ProjectId == projectId)
            .ToListAsync();
        return epics.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Issue>> GetChildIssuesOfEpicByEpicIdAsync(Guid epicId)
    {
        var childIssues = await Entity
            .Where(e => e.ParentId == epicId)
            .ToListAsync();

        return childIssues.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Issue>> GetChildIssuesOfIssueByIssueIdAsync(Guid issueId)
    {
        var childIssues = await Entity
            .Where(e => e.ParentId == issueId)
            .ToListAsync();

        return childIssues.AsReadOnly();
    }

    public async Task LoadIssueTypeAsync(Issue issue)
    {
        await Entity.Entry(issue).Reference(i => i.IssueType).LoadAsync();
    }

    public async Task LoadIssueDetailAsync(Issue issue)
    {
        await Entity.Entry(issue).Reference(i => i.IssueDetail).LoadAsync();
    }

    public async Task LoadAttachmentsAsync(Issue issue)
    {
        await Entity.Entry(issue).Collection(i => i.Attachments!).LoadAsync();
    }

    public async Task LoadStatusAsync(Issue issue)
    {
        await Entity.Entry(issue).Reference(i => i.Status).LoadAsync();
    }

    public async Task<string> GetParentNameAsync(Guid parentId)
    {
        var parentName = await Entity
            .AsNoTracking()
            .Where(i => i.Id == parentId)
            .Select(i => i.Name)
            .FirstOrDefaultAsync() ?? string.Empty;

        return parentName;
    }

    public async Task<IReadOnlyCollection<Issue>> GetIssuesOfVersionByVersionIdAsync(Guid versionId)
    {
        var childIssues = await Entity
            //.Where(i => i.VersionId == versionId)
            .ToListAsync();
        return childIssues.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Issue>> GetNotDoneIssuesBySprintIdAsync(Guid sprintId, Guid projectId)
    {
        var statusCategoryCodes = new List<string>()
        {
            StatusCategoryConstants.ToDoCode, StatusCategoryConstants.InProgressCode
        };
        var query = from sc in DbContext.Set<StatusCategory>().Where(sc => statusCategoryCodes.Contains(sc.Code))
                    join s in DbContext.Set<Status>().Where(s => s.ProjectId == projectId) on sc.Id equals s.StatusCategoryId
                    join i in Entity.Where(i => i.SprintId == sprintId) on s.Id equals i.StatusId
                    select i;

        var issues = await query.ToListAsync();
        return issues.AsReadOnly();
    }

    public async Task<string> GetNameOfIssueAsync(Guid issueId)
    {
        var name = await Entity
            .AsNoTracking()
            .Where(i => i.Id == issueId)
            .Select(i => i.Name)
            .FirstOrDefaultAsync() ?? string.Empty;

        return name;
    }

    public async Task DeleteByBacklogIdAsync(Guid backlogId)
    {
        var issues = await Entity
            .Where(i => i.BacklogId == backlogId)
            .ToListAsync();

        foreach (var issue in issues)
        {
            await LoadEntitiesRelationshipAsync(issue);
            Entity.Remove(issue);
        }
    }

    public async Task DeleteBySprintIdAsync(Guid sprintId)
    {
        var issues = await Entity
            .Where(i => i.SprintId == sprintId)
            .ToListAsync();

        foreach (var issue in issues)
        {
            await LoadEntitiesRelationshipAsync(issue);
            Entity.Remove(issue);
        }
    }

    public async Task DeleteByProjectIdAsync(Guid projectId)
    {
        var issues = await Entity
            .Where(i => i.ProjectId == projectId)
            .ToListAsync();

        foreach (var issue in issues)
        {
            await LoadEntitiesRelationshipAsync(issue);
            Entity.Remove(issue);
        }
    }

    public async Task<IReadOnlyCollection<Guid>?> GetAllWatcherOfIssueAsync(Guid issueId)
    {
        var watcher = await Entity
            .AsNoTracking()
            .Where(i => i.Id == issueId)
            .Select(i => i.Watcher)
            .FirstOrDefaultAsync();

        if (watcher is null) return null;

        var watcherIds = watcher.Users!.Select(u => u.Identity).ToList();

        return watcherIds.AsReadOnly();
    }

    public async Task<string> GetProjectNameOfIssueAsync(Guid issueId)
    {
        var issue = await Entity
            .AsNoTracking()
            .Where(i => i.Id == issueId)
            .FirstOrDefaultAsync();

        if (issue is not null && issue.SprintId is Guid sprintId)
        {
            var projectId = await DbContext.Set<Sprint>()
                .AsNoTracking()
                .Where(s => s.Id == sprintId)
                .Select(s => s.ProjectId)
                .FirstOrDefaultAsync();

            return await DbContext.Set<Project>()
                .AsNoTracking()
                .Where(s => s.Id == projectId)
                .Select(s => s.Name)
                .FirstOrDefaultAsync() ?? string.Empty;
        }
        else if (issue is not null && issue.BacklogId is Guid backlogId)
        {
            var projectId = await DbContext.Set<Backlog>()
                .AsNoTracking()
                .Where(s => s.Id == backlogId)
                .Select(s => s.ProjectId)
                .FirstOrDefaultAsync();

            return await DbContext.Set<Project>()
                .AsNoTracking()
                .Where(s => s.Id == projectId)
                .Select(s => s.Name)
                .FirstOrDefaultAsync() ?? string.Empty;
        }
        else if (issue is not null && issue.ProjectId is Guid projectId)
        {
            return await DbContext.Set<Project>()
                .AsNoTracking()
                .Where(s => s.Id == projectId)
                .Select(s => s.Name)
                .FirstOrDefaultAsync() ?? string.Empty;
        }
        else
        {
            return string.Empty;
        }
    }

    public async Task UpdateOneColumnForIssueAsync(Guid oldValue, Guid? newValue, NameColumn nameColumn)
    {
        switch (nameColumn)
        {
            case NameColumn.PriorityId:
                await Entity
                    .Where(i => i.PriorityId == oldValue)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.PriorityId, newValue));
                break;
            case NameColumn.ParentId:
                await Entity
                    .Where(i => i.ParentId == oldValue)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.ParentId, newValue));
                break;
            case NameColumn.StatusId:
                await Entity
                    .Where(i => i.StatusId == oldValue)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.StatusId, newValue));
                break;
            case NameColumn.IssueTypeId:
                await Entity
                    .Where(i => i.IssueTypeId == oldValue)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.IssueTypeId, newValue));
                break;
        }
    }

    public async Task<int> CountIssueByPriorityIdAsync(Guid priorityId)
    {
        return await Entity
            .Where(i => i.PriorityId == priorityId)
            .CountAsync();
    }

    public async Task<int> CountIssueByStatusIdAsync(Guid statusId)
    {
        return await Entity
            .Where(i => i.StatusId == statusId)
            .CountAsync();
    }

    public async Task<int> CountIssueByIssueTypeIdAsync(Guid issueTypeId)
    {
        return await Entity
            .Where(i => i.IssueTypeId == issueTypeId)
            .CountAsync();
    }

    public async Task<Guid> GetProjectIdOfIssueAsync(Guid issueId)
    {
        var issue = await Entity
            .AsNoTracking()
            .Where(i => i.Id == issueId)
            .FirstOrDefaultAsync();

        if (issue is not null && issue.SprintId is Guid sprintId)
        {
            return await DbContext.Set<Sprint>()
                .AsNoTracking()
                .Where(s => s.Id == sprintId)
                .Select(s => s.ProjectId)
                .FirstOrDefaultAsync();
        }
        else if (issue is not null && issue.BacklogId is Guid backlogId)
        {
            return await DbContext.Set<Backlog>()
                .AsNoTracking()
                .Where(s => s.Id == backlogId)
                .Select(s => s.ProjectId)
                .FirstOrDefaultAsync();
        }
        else if (issue is not null && issue.ProjectId is Guid projectId)
        {
            return projectId;
        }
        else
        {
            return Guid.Empty;
        }
    }

    public async Task<Guid> GetProjectLeadIdOfIssueAsync(Guid issueId)
    {
        var issue = await Entity
            .AsNoTracking()
            .Where(i => i.Id == issueId)
            .FirstOrDefaultAsync();

        if (issue is not null && issue.SprintId is Guid sprintId)
        {
            var projectId = await DbContext.Set<Sprint>()
                .AsNoTracking()
                .Where(s => s.Id == sprintId)
                .Select(s => s.ProjectId)
                .FirstOrDefaultAsync();

            return await DbContext.Set<UserProject>()
                .Where(up => up.ProjectId == projectId && up.Role == RoleConstants.LeaderRole)
                .Select(up => up.UserId)
                .FirstOrDefaultAsync();
        }
        else if (issue is not null && issue.BacklogId is Guid backlogId)
        {
            var projectId = await DbContext.Set<Backlog>()
                .AsNoTracking()
                .Where(s => s.Id == backlogId)
                .Select(s => s.ProjectId)
                .FirstOrDefaultAsync();

            return await DbContext.Set<UserProject>()
                .Where(up => up.ProjectId == projectId && up.Role == RoleConstants.LeaderRole)
                .Select(up => up.UserId)
                .FirstOrDefaultAsync();
        }
        else if (issue is not null && issue.ProjectId is Guid projectId)
        {
            return await DbContext.Set<UserProject>()
                .Where(up => up.ProjectId == projectId && up.Role == RoleConstants.LeaderRole)
                .Select(up => up.UserId)
                .FirstOrDefaultAsync();
        }
        else
        {
            return Guid.Empty;
        }
    }

    public async Task<string> GetProjectCodeOfIssueAsync(Guid issueId)
    {
        var issue = await Entity
            .AsNoTracking()
            .Where(i => i.Id == issueId)
            .FirstOrDefaultAsync();

        if (issue is not null && issue.SprintId is Guid sprintId)
        {
            var projectId = await DbContext.Set<Sprint>()
                .AsNoTracking()
                .Where(s => s.Id == sprintId)
                .Select(s => s.ProjectId)
                .FirstOrDefaultAsync();

            return await DbContext.Set<Project>()
                .AsNoTracking()
                .Where(s => s.Id == projectId)
                .Select(s => s.Code)
                .FirstOrDefaultAsync() ?? string.Empty;
        }
        else if (issue is not null && issue.BacklogId is Guid backlogId)
        {
            var projectId = await DbContext.Set<Backlog>()
                .AsNoTracking()
                .Where(s => s.Id == backlogId)
                .Select(s => s.ProjectId)
                .FirstOrDefaultAsync();

            return await DbContext.Set<Project>()
                .AsNoTracking()
                .Where(s => s.Id == projectId)
                .Select(s => s.Code)
                .FirstOrDefaultAsync() ?? string.Empty;
        }
        else if (issue is not null && issue.ProjectId is Guid projectId)
        {
            return await DbContext.Set<Project>()
                .AsNoTracking()
                .Where(s => s.Id == projectId)
                .Select(s => s.Code)
                .FirstOrDefaultAsync() ?? string.Empty;
        }
        else
        {
            return string.Empty;
        }
    }

    public async Task<Project> GetProjectByIssueIdAsync(Guid issueId)
    {
        var issue = await Entity
            .AsNoTracking()
            .Where(i => i.Id == issueId)
            .FirstOrDefaultAsync();

        if (issue?.SprintId is Guid sprintId)
        {
            var project = await (from s in DbContext.Set<Sprint>().AsNoTracking().Where(s => s.Id == issue.SprintId)
                                 join p in DbContext.Set<Project>().AsNoTracking() on s.ProjectId equals p.Id
                                 select p)
                                 .FirstOrDefaultAsync();

            return project!;
        }
        else if (issue?.BacklogId is Guid backlogId)
        {
            var project = await (from b in DbContext.Set<Backlog>().AsNoTracking().Where(s => s.Id == issue.BacklogId)
                                 join p in DbContext.Set<Project>().AsNoTracking() on b.ProjectId equals p.Id
                                 select p)
                                 .FirstOrDefaultAsync();

            return project!;
        }
        // issue?.ProjectId is Guid projectId
        else
        {
            var project = await (from i in Entity.AsNoTracking()
                                 join p in DbContext.Set<Project>().AsNoTracking() on i.ProjectId equals p.Id
                                 select p)
                                 .FirstOrDefaultAsync();

            return project!;
        }
    }

    public async Task DeleteChildIssueAsync(Guid parentId)
    {
        await Entity
            .Where(i => i.ParentId == parentId)
            .ExecuteDeleteAsync();
    }
}
