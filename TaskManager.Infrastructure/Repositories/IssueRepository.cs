using Microsoft.EntityFrameworkCore;
using TaskManager.Core;
using TaskManager.Core.Core;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class IssueRepository : IIssueRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public IssueRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Issue Add(Issue issue)
    {
        return _context.Issues.Add(issue).Entity;
    }

    public void Delete(Guid id)
    {
        var issue = _context.Issues.SingleOrDefault(e => e.Id == id);
        _context.Issues.Remove(issue!);
    }

    public void Update(Issue issue)
    {
        _context.Entry(issue).State = EntityState.Modified;
    }

    public async Task<Issue> Get(Guid id)
    {
        var issue = await _context.Issues.FirstOrDefaultAsync(e => e.Id == id);
        return issue!;
    }

    public async Task<IReadOnlyCollection<Issue>> GetIssueBySprintId(Guid sprintId)
    {
        var issues = await _context.Issues
            .Where(i => i.SprintId == sprintId)
            .ToListAsync();
        return issues.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Issue>> GetIssueByBacklogId(Guid backlogId)
    {
        var issues = await _context.Issues
            .Where(i => i.BacklogId == backlogId)
            .ToListAsync();
        return issues.AsReadOnly();
    }

    public void UpdateRange(IReadOnlyCollection<Issue> issues)
    {
        _context.Issues.UpdateRange(issues);
    }

    public async Task<IReadOnlyCollection<Issue>> GetChildIssues(Guid parentId)
    {
        var childIssues = await _context.Issues
            .Where(i => i.ParentId == parentId)
            .ToListAsync();
        return childIssues.AsReadOnly();
    }

    public void DeleteRange(IReadOnlyCollection<Issue> issues)
    {
        _context.Issues.RemoveRange(issues);
    }

    public async Task LoadEntitiesRelationship(Issue issue)
    {
        await _context.Entry(issue).Reference(i => i.IssueType).LoadAsync();
        await _context.Entry(issue).Reference(i => i.IssueDetail).LoadAsync();
        await _context.Entry(issue).Collection(i => i.Attachments!).LoadAsync();
        await _context.Entry(issue).Collection(i => i.IssueHistories!).LoadAsync();
        await _context.Entry(issue).Collection(i => i.Comments!).LoadAsync();
        await _context.Entry(issue).Reference(i => i.Status).LoadAsync();
    }

    public async Task<IReadOnlyCollection<Issue>> GetByIds(IReadOnlyCollection<Guid> ids)
    {
        var issues = await _context.Issues.Where(e => ids.Contains(e.Id)).ToListAsync();
        return issues.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Issue>> GetEpicByProjectId(Guid projectId)
    {
        var epics = await _context.Issues.Where(e => e.SprintId == null && e.BacklogId == null && e.ProjectId == projectId).ToListAsync();
        return epics.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Issue>> GetChildIssueOfEpic(Guid epicId)
    {
        var childIssues = await _context.Issues.Where(e => e.ParentId == epicId).ToListAsync();
        return childIssues.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Issue>> GetChildIssueOfIssue(Guid issueId)
    {
        var childIssues = await _context.Issues.Where(e => e.ParentId == issueId).ToListAsync();
        return childIssues.AsReadOnly();
    }

    public async Task LoadComments(Issue issue)
    {
        await _context.Entry(issue).Collection(i => i.Comments!).LoadAsync();
    }

    public async Task LoadIssueType(Issue issue)
    {
        await _context.Entry(issue).Reference(i => i.IssueType).LoadAsync();
    }

    public async Task LoadIssueDetail(Issue issue)
    {
        await _context.Entry(issue).Reference(i => i.IssueDetail).LoadAsync();
    }

    public async Task LoadAttachments(Issue issue)
    {
        await _context.Entry(issue).Collection(i => i.Attachments!).LoadAsync();
    }

    public async Task LoadIssueHistories(Issue issue)
    {
        await _context.Entry(issue).Collection(i => i.IssueHistories!).LoadAsync();
    }

    public async Task LoadStatus(Issue issue)
    {
        await _context.Entry(issue).Reference(i => i.Status).LoadAsync();
    }

    public async Task<string> GetParentName(Guid parentId)
    {
        var parentName = await _context.Issues.AsNoTracking().Where(i => i.Id == parentId).Select(i => i.Name).FirstOrDefaultAsync();
        if (string.IsNullOrWhiteSpace(parentName))
        {
            return string.Empty;
        }
        return parentName;
    }

    public async Task<IReadOnlyCollection<Issue>> GetCreatedAWeekAgo()
    {
        var issues = await _context.Issues.Where(i => i.CreationTime >= DateTime.Now.AddDays(-7)).ToListAsync();
        return issues.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Issue>> GetResolvedAWeekAgo()
    {
        var issues = await _context.Issues.Where(i => i.CompleteDate >= DateTime.Now.AddDays(-7)).ToListAsync();
        return issues.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Issue>> GetUpdatedAWeekAgo()
    {
        var issues = await _context.Issues.Where(i => i.ModificationTime >= DateTime.Now.AddDays(-7)).ToListAsync();
        return issues.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Issue>> GetChildIssueOfVersion(Guid versionId)
    {
        var childIssues = await _context.Issues
            //.Where(i => i.VersionId == versionId)
            .ToListAsync();
        return childIssues.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Issue>> GetNotDoneIssuesBySprintId(Guid sprintId, Guid projectId)
    {
        var statusCategoryCodes = new List<string>()
        {
            CoreConstants.ToDoCode, CoreConstants.InProgressCode
        };
        var query = from sc in _context.StatusCategories.Where(sc => statusCategoryCodes.Contains(sc.Code))
                    join s in _context.Statuses.Where(s => s.ProjectId == projectId) on sc.Id equals s.StatusCategoryId
                    join i in _context.Issues.Where(i => i.SprintId == sprintId) on s.Id equals i.StatusId
                    select i;

        var issues = await query.ToListAsync();
        return issues.AsReadOnly();
    }

    public async Task<string?> GetNameOfIssue(Guid issueId)
    {
        var name = await _context.Issues.AsNoTracking().Where(i => i.Id == issueId).Select(i => i.Name).FirstOrDefaultAsync();
        return name;
    }

    public async Task DeleteByBacklogId(Guid backlogId)
    {
        var issues = await _context.Issues.Where(i => i.BacklogId == backlogId).ToListAsync();
        foreach (var issue in issues)
        {
            await LoadEntitiesRelationship(issue);
            _context.Issues.Remove(issue);
        }
        await UnitOfWork.SaveChangesAsync();
    }

    public async Task DeleteBySprintId(Guid sprintId)
    {
        var issues = await _context.Issues.Where(i => i.SprintId == sprintId).ToListAsync();
        foreach (var issue in issues)
        {
            await LoadEntitiesRelationship(issue);
            _context.Issues.Remove(issue);
        }
        await UnitOfWork.SaveChangesAsync();
    }

    public async Task DeleteByProjectId(Guid projectId)
    {
        var issues = await _context.Issues.Where(i => i.ProjectId == projectId).ToListAsync();
        foreach (var issue in issues)
        {
            await LoadEntitiesRelationship(issue);
            _context.Issues.Remove(issue);
        }
        await UnitOfWork.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<Guid>?> GetAllWatcherOfIssue(Guid issueId)
    {
        var watcher = await _context.Issues.AsNoTracking().Where(i => i.Id == issueId).Select(i => i.Watcher).FirstOrDefaultAsync();
        if (watcher is null) return null;
        var watcherIds = watcher.Users!.Select(u => u.Identity).ToList();
        return watcherIds.AsReadOnly();
    }

    public async Task<string> GetProjectNameOfIssue(Guid issueId)
    {
        var issue = await _context.Issues.AsNoTracking().Where(i => i.Id == issueId).FirstOrDefaultAsync();
        if (issue is not null && issue.SprintId is Guid sprintId)
        {
            var projectId = await _context.Sprints.AsNoTracking().Where(s => s.Id == sprintId).Select(s => s.ProjectId).FirstOrDefaultAsync();
            return await _context.Projects.AsNoTracking().Where(s => s.Id == projectId).Select(s => s.Name).FirstOrDefaultAsync() ?? string.Empty;
        }
        else if (issue is not null && issue.BacklogId is Guid backlogId)
        {
            var projectId = await _context.Backlogs.AsNoTracking().Where(s => s.Id == backlogId).Select(s => s.ProjectId).FirstOrDefaultAsync();
            return await _context.Projects.AsNoTracking().Where(s => s.Id == projectId).Select(s => s.Name).FirstOrDefaultAsync() ?? string.Empty;
        }
        else if (issue is not null && issue.ProjectId is Guid projectId)
        {
            return await _context.Projects.AsNoTracking().Where(s => s.Id == projectId).Select(s => s.Name).FirstOrDefaultAsync() ?? string.Empty;
        }
        else
        {
            return string.Empty;
        }
    }

    public async Task UpdateOneColumnForIssue(Guid oldValue, Guid? newValue, NameColumn nameColumn)
    {
        switch (nameColumn)
        {
            case NameColumn.PriorityId:
                await _context.Issues
                    .Where(i => i.PriorityId == oldValue)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.PriorityId, newValue));
                break;
            case NameColumn.ParentId:
                await _context.Issues
                    .Where(i => i.ParentId == oldValue)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.ParentId, newValue));
                break;
            case NameColumn.StatusId:
                await _context.Issues
                    .Where(i => i.StatusId == oldValue)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.StatusId, newValue));
                break;
            case NameColumn.IssueTypeId:
                await _context.Issues
                    .Where(i => i.IssueTypeId == oldValue)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.IssueTypeId, newValue));
                break;
        }
    }

    public async Task<int> CountIssueByPriorityId(Guid priorityId)
    {
        return await _context.Issues.Where(i => i.PriorityId == priorityId).CountAsync();
    }

    public async Task<int> CountIssueByStatusId(Guid statusId)
    {
        return await _context.Issues.Where(i => i.StatusId == statusId).CountAsync();
    }

    public async Task<int> CountIssueByIssueTypeId(Guid issueTypeId)
    {
        return await _context.Issues.Where(i => i.IssueTypeId == issueTypeId).CountAsync();
    }

    public async Task<Guid> GetProjectIdOfIssue(Guid issueId)
    {
        var issue = await _context.Issues.AsNoTracking().Where(i => i.Id == issueId).FirstOrDefaultAsync();
        if (issue is not null && issue.SprintId is Guid sprintId)
        {
            return await _context.Sprints.AsNoTracking().Where(s => s.Id == sprintId).Select(s => s.ProjectId).FirstOrDefaultAsync();
        }
        else if (issue is not null && issue.BacklogId is Guid backlogId)
        {
            return await _context.Backlogs.AsNoTracking().Where(s => s.Id == backlogId).Select(s => s.ProjectId).FirstOrDefaultAsync();
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

    public async Task<Guid> GetProjectLeadIdOfIssue(Guid issueId)
    {
        var issue = await _context.Issues.AsNoTracking().Where(i => i.Id == issueId).FirstOrDefaultAsync();
        if (issue is not null && issue.SprintId is Guid sprintId)
        {
            var projectId = await _context.Sprints.AsNoTracking().Where(s => s.Id == sprintId).Select(s => s.ProjectId).FirstOrDefaultAsync();

            return await _context.UserProjects.Where(up => up.ProjectId == projectId && up.Role == CoreConstants.LeaderRole).Select(up => up.UserId).FirstOrDefaultAsync();
        }
        else if (issue is not null && issue.BacklogId is Guid backlogId)
        {
            var projectId = await _context.Backlogs.AsNoTracking().Where(s => s.Id == backlogId).Select(s => s.ProjectId).FirstOrDefaultAsync();
            return await _context.UserProjects.Where(up => up.ProjectId == projectId && up.Role == CoreConstants.LeaderRole).Select(up => up.UserId).FirstOrDefaultAsync();
        }
        else if (issue is not null && issue.ProjectId is Guid projectId)
        {
            return await _context.UserProjects.Where(up => up.ProjectId == projectId && up.Role == CoreConstants.LeaderRole).Select(up => up.UserId).FirstOrDefaultAsync();
        }
        else
        {
            return Guid.Empty;
        }
    }

    public async Task<string> GetProjectCodeOfIssue(Guid issueId)
    {
        var issue = await _context.Issues.AsNoTracking().Where(i => i.Id == issueId).FirstOrDefaultAsync();
        if (issue is not null && issue.SprintId is Guid sprintId)
        {
            var projectId = await _context.Sprints.AsNoTracking().Where(s => s.Id == sprintId).Select(s => s.ProjectId).FirstOrDefaultAsync();
            return await _context.Projects.AsNoTracking().Where(s => s.Id == projectId).Select(s => s.Code).FirstOrDefaultAsync() ?? string.Empty;
        }
        else if (issue is not null && issue.BacklogId is Guid backlogId)
        {
            var projectId = await _context.Backlogs.AsNoTracking().Where(s => s.Id == backlogId).Select(s => s.ProjectId).FirstOrDefaultAsync();
            return await _context.Projects.AsNoTracking().Where(s => s.Id == projectId).Select(s => s.Code).FirstOrDefaultAsync() ?? string.Empty;
        }
        else if (issue is not null && issue.ProjectId is Guid projectId)
        {
            return await _context.Projects.AsNoTracking().Where(s => s.Id == projectId).Select(s => s.Code).FirstOrDefaultAsync() ?? string.Empty;
        }
        else
        {
            return string.Empty;
        }
    }
}
