namespace TaskManager.Persistence.Repositories;

public class ProjectRepository(IDbContext context) : GenericRepository<Project>(context)
    , IProjectRepository
{
    public async Task<PaginationResult<Project>> GetProjectsByUserIdAsync(Guid userId, GetProjectByFilterDto filter, PaginationInput paginationInput)
    {

        var query = from u in DbContext.AppUser.AsNoTracking()
                    join up in DbContext.Set<UserProject>().AsNoTracking().Where(up => up.UserId == userId) on u.Id equals up.UserId
                    join p in Entity.AsNoTracking()
                    .WhereIf(!string.IsNullOrWhiteSpace(filter.name), p => p.Name.Contains(filter.name))
                    .WhereIf(!string.IsNullOrWhiteSpace(filter.code), p => p.Code.Contains(filter.code))
                    on up.ProjectId equals p.Id
                    select p;

        return await query.Pagination(paginationInput);
    }

    public async Task<IReadOnlyCollection<Project>> GetProjectsByUserIdPagingAsync(Guid userId, GetProjectByFilterDto filter)
    {
        var query = from u in DbContext.AppUser.AsNoTracking()
                    join up in DbContext.Set<UserProject>().AsNoTracking().Where(up => up.UserId == userId) on u.Id equals up.UserId
                    join p in Entity.AsNoTracking()
                    .WhereIf(!string.IsNullOrWhiteSpace(filter.name), p => p.Name.Contains(filter.name))
                    .WhereIf(!string.IsNullOrWhiteSpace(filter.code), p => p.Code.Contains(filter.code))
                    on up.ProjectId equals p.Id
                    select p;

        var projects = await query.ToListAsync();
        return projects.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<UserViewModel>> GetMembersAsync(Guid projectId)
    {
        var members = await (from up in DbContext.Set<UserProject>().AsNoTracking().Where(up => up.ProjectId == projectId)
                             join u in DbContext.AppUser.AsNoTracking() on up.UserId equals u.Id
                             select new UserViewModel
                             {
                                 Id = u.Id,
                                 Name = u.Name,
                                 Department = u.Department,
                                 Organization = u.Organization,
                                 AvatarUrl = u.AvatarUrl,
                                 JobTitle = u.JobTitle,
                                 Location = u.Location,
                                 Email = u.Email,
                                 Role = up.Role
                             }).ToListAsync();

        return members.AsReadOnly();
    }

    public async Task<Project?> GetProjectByCodeAsync(string code)
    {
        var project = await Entity
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Code == code);

        return project;
    }

    public async Task LoadIssueTypesAsync(Project project)
    {
        await Entity.Entry(project).Collection(p => p.IssueTypes!).LoadAsync();
    }

    public async Task LoadStatusesAsync(Project project)
    {
        await Entity.Entry(project).Collection(p => p.Statuses!).LoadAsync();
    }

    public async Task LoadBacklogAsync(Project project)
    {
        await Entity.Entry(project).Reference(p => p.Backlog).LoadAsync();
    }

    public async Task LoadUserProjectsAsync(Project project)
    {
        await Entity.Entry(project).Collection(p => p.UserProjects!).LoadAsync();
    }

    public async Task LoadProjectConfigurationAsync(Project project)
    {
        await Entity.Entry(project).Reference(p => p.ProjectConfiguration).LoadAsync();
    }

    public async Task LoadTransitionAsync(Project project)
    {
        await Entity.Entry(project).Collection(p => p.Transitions!).LoadAsync();
    }

    public async Task LoadWorkflowAsync(Project project)
    {
        await Entity.Entry(project).Reference(p => p.Workflow).LoadAsync();
    }

    public async Task LoadPrioritiesAsync(Project project)
    {
        await Entity.Entry(project).Collection(p => p.Priorities!).LoadAsync();
    }

    public async Task LoadPermissionGroupsAsync(Project project)
    {
        await Entity.Entry(project).Collection(p => p.PermissionGroups!).LoadAsync();
    }

    public async Task LoadSprintsAsync(Project project)
    {
        await Entity.Entry(project).Collection(p => p.Sprints!).LoadAsync();
    }

    public async Task LoadVersionsAsync(Project project)
    {
        await Entity.Entry(project).Collection(p => p.Versions!).LoadAsync();
    }

    public async Task<string> GetProjectNameAsync(Guid projectId)
    {
        var name = await Entity.Where(p => p.Id == projectId).Select(p => p.Name).FirstOrDefaultAsync();
        return name!;
    }

    public async Task<IReadOnlyCollection<SprintFilterViewModel>> GetSprintFiltersByProjectIdAsync(Guid projectId)
    {
        var sprintFilterViewModels = await DbContext.Set<Sprint>()
            .AsNoTracking()
            .Where(s => s.ProjectId == projectId && s.IsStart == true)
            .Select(s => new SprintFilterViewModel
            {
                Id = s.Id,
                Name = s.Name
            })
            .ToListAsync();

        return sprintFilterViewModels.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<EpicFilterViewModel>> GetEpicFiltersByProjectIdAsync(Guid projectId)
    {
        var epicFilterViewModels = await (from s in DbContext.Set<Sprint>().Where(s => s.ProjectId == projectId && s.IsStart == true)
                                          join i in DbContext.Set<Issue>() on s.Id equals i.SprintId
                                          join e in DbContext.Set<Issue>() on i.ParentId equals e.Id
                                          group e by new { e.Id, e.Name } into g
                                          select new EpicFilterViewModel
                                          {
                                              Id = g.Key.Id,
                                              Name = g.Key.Name,
                                          }).ToArrayAsync();

        return epicFilterViewModels.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<LabelFilterViewModel>> GetLabelFiltersByProjectIdAsync(Guid projectId)
    {
        var epicFilterViewModels = await (from s in DbContext.Set<Sprint>().Where(s => s.ProjectId == projectId && s.IsStart == true)
                                          join i in DbContext.Set<Issue>() on s.Id equals i.SprintId
                                          join li in DbContext.Set<LabelIssue>() on i.Id equals li.IssueId
                                          join l in DbContext.Set<Label>() on li.LabelId equals l.Id
                                          group l by new { l.Id, l.Name } into g
                                          select new LabelFilterViewModel
                                          {
                                              Id = g.Key.Id,
                                              Name = g.Key.Name,
                                          })
                                          .ToArrayAsync();

        return epicFilterViewModels.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<TypeFilterViewModel>> GetIssueTypeFiltersByProjectIdAsync(Guid projectId)
    {
        var epicFilterViewModels = await (from s in DbContext.Set<Sprint>().Where(s => s.ProjectId == projectId && s.IsStart == true)
                                          join i in DbContext.Set<Issue>() on s.Id equals i.SprintId
                                          join it in DbContext.Set<IssueType>() on i.IssueTypeId equals it.Id
                                          group it by new { it.Id, it.Name } into g
                                          select new TypeFilterViewModel
                                          {
                                              Id = g.Key.Id,
                                              Name = g.Key.Name,
                                          }).ToArrayAsync();

        return epicFilterViewModels.AsReadOnly();
    }

    public Task<PaginationResult<Project>> GetProjectsByUserIdPagingAsync(Guid userId, GetProjectByFilterDto filter, PaginationInput paginationInput)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Project>> GetProjectsByUserIdAsync(Guid userId, GetProjectByFilterDto input)
    {
        throw new NotImplementedException();
    }
}